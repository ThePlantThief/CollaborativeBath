using CollaborativeBath.Models;
using CollaborativeBath.ViewModels;
using CollaborativeBath.ViewModels.Material;
using Microsoft.AspNet.Identity;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Configuration;
using System.Web.Mvc;
using File = CollaborativeBath.Models.File;

namespace CollaborativeBath.Controllers
{
    public class FileController : Controller
    {
        #region Public Methods

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Delete(int? id)
        {
            int folderId = 0;
            if (ModelState.IsValid)
            {
                using (MaterialContext db = new MaterialContext())
                {
                    ApplicationUser user = db.Users.Find(User.Identity.GetUserId());
                    var item = db.Files
                        .Include(c => c.VoteList.Votes)
                        .Include(c => c.CommentList.Comments.Select(l => l.VoteList.Votes))
                        .Include(c => c.Parent)
                        .FirstOrDefault(c => c.Id == id);
                    folderId = item.Parent.Id;
                    if (item != null && item.User == user)
                    {
                        CloudBlockBlob blob = GetCloudBlobContainer().GetBlockBlobReference(item.Id.ToString());
                        blob.Delete();
                        db.Files.Remove(item);
                        db.SaveChanges();
                    }

                    db.SaveChanges();
                }
            }

            return RedirectToAction("Details", "Folder", new { id = folderId });
        }

        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            List<int> history = new List<int>();
            File file = null;
            using (MaterialContext db = new MaterialContext())
            {
                file = db.Files
                    .Include(i => i.Parent)
                    .Include(i => i.User)
                    .Include(i => i.ParentFile)
                    .Include(i => i.VoteList.Votes)
                    .Include(i => i.CommentList.Comments.Select(c => c.User))
                    .Include(u => u.CommentList.Comments.Select(c => c.VoteList.Votes))
                    .FirstOrDefault(c => c.Id == id);
                int? parentId = file?.ParentFile?.Id;
                history.Add(file.Id);
                while (parentId != null)
                {
                    history.Add(parentId.Value);
                    parentId = db.Files.Include(i => i.ParentFile).FirstOrDefault(i => i.Id == parentId)?.ParentFile?.Id;
                }
            }

            if (file.Parent == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(new FileDetailsViewModel(file, GetParents(file.Parent.Id), history.ToArray()));
            }
        }

        public FileContentResult GetFile(int? id)
        {
            if (id != null)
            {
                File file = null;
                using (MaterialContext db = new MaterialContext())
                {
                    file = db.Files.Find(id);
                    if (file == null)
                    {
                        return null;
                    }
                }
                CloudBlockBlob blob = GetCloudBlobContainer().GetBlockBlobReference(file.Id.ToString());
                MemoryStream ms = new MemoryStream();
                blob.DownloadToStreamAsync(ms).Wait();
                byte[] fileBytes = ms.ToArray();
                return File(fileBytes, file.Type);
            }
            else
            {
                return null;
            }
        }

        public ActionResult GetHistory(int? id)
        {
            IList<int> history = new List<int>();
            using (MaterialContext db = new MaterialContext())
            {
                history = new List<int>();
                var file = db.Files
                    .Include(i => i.ParentFile)
                    .FirstOrDefault(c => c.Id == id);
                int? parentId = file?.ParentFile?.Id;
                history.Add(file.Id);
                while (parentId != null)
                {
                    history.Add(parentId.Value);
                    parentId = db.Files.Include(i => i.ParentFile).FirstOrDefault(i => i.Id == parentId)?.ParentFile?.Id;
                }
            }
            return Json(history.ToArray(), JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult Reupload(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            FileUploadViewModel model = new FileUploadViewModel();
            using (MaterialContext db = new MaterialContext())
            {
                File file = db.Files
                    .Include(f => f.Parent)
                    .FirstOrDefault(f => f.Id == id);
                model.FolderId = file.Parent.Id;
                model.Title = file.Title;
                model.Id = file.Id;
            }

            return View(model);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Reupload(FileUploadViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return null;
            }

            int newId = model.Id;
            bool updated = false;
            File file = null;
            using (MaterialContext db = new MaterialContext())
            {
                byte[] fileData = null;
                string type = model.File.ContentType;
                File parent = db.Files
                    .Include(f => f.User)
                    .Include(f => f.Parent)
                    .Include(f => f.CommentList)
                    .Include(f => f.VoteList)
                    .FirstOrDefault(f => f.Id == model.Id);
                ApplicationUser user = db.Users.Find(User.Identity.GetUserId());
                if (parent?.User.Id == User.Identity.GetUserId())
                {
                    file = new File()
                    {
                        Title = model.Title,
                        Type = type,
                        CommentList = parent.CommentList,
                        VoteList = parent.VoteList,
                        Parent = parent.Parent,
                        ParentFile = parent,
                        User = user
                    };
                    parent.Parent = null;
                    db.Files.Add(file);
                    db.SaveChanges();
                    newId = file.Id;
                    updated = true;
                }
            }

            if (updated)
            {
                CloudBlockBlob blob = GetCloudBlobContainer().GetBlockBlobReference(file.Id.ToString());
                blob.UploadFromStreamAsync(model.File.InputStream).Wait();
                NotificationController.UpdatedFile(file);
            }
            return RedirectToAction("Details", "File", new { id = newId });
        }

        [Authorize]
        public ActionResult Upload(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            return View(new FileUploadViewModel() { FolderId = id.Value });
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Upload(FileUploadViewModel fileUploadViewModel)
        {
            if (ModelState.IsValid)
            {
                File file = null;
                using (MaterialContext db = new MaterialContext())
                {
                    ApplicationUser user = db.Users.Find(User.Identity.GetUserId());
                    string type = fileUploadViewModel.File.ContentType;
                    Folder folder = db.Folders.Find(fileUploadViewModel.FolderId);
                    file = new File()
                    {
                        Title = fileUploadViewModel.Title,
                        Type = type,
                        VoteList = new VoteList(),
                        CommentList = new CommentList(),
                        User = user
                    };
                    folder.Items.Add(file);
                    file.VoteList.SubscribedUsers.Add(user);
                    file.CommentList.SubscribedUsers.Add(user);
                    db.SaveChanges();
                }
                CloudBlockBlob blob = GetCloudBlobContainer().GetBlockBlobReference(file.Id.ToString());
                blob.UploadFromStreamAsync(fileUploadViewModel.File.InputStream).Wait();
                NotificationController.NewFile(file);

                return RedirectToAction("Details", "File", new { id = file.Id });
            }
            else
            {
                return null;
            }
        }

        #endregion Public Methods

        #region Protected Methods

        [Authorize]
        protected Breadcrumb[] GetParents(int? id)
        {
            List<Breadcrumb> parents = new List<Breadcrumb>();
            int? currentId = id;
            using (MaterialContext db = new MaterialContext())
            {
                while (currentId != null)
                {
                    var folder = db.Folders.Find(currentId);
                    parents.Add(new Breadcrumb()
                    {
                        Title = folder.Title,
                        Id = folder.Id,
                    });
                    currentId = folder.ParentId;
                }
            }

            parents.Reverse();
            return parents.ToArray();
        }

        #endregion Protected Methods

        #region Private Methods

        private CloudBlobContainer GetCloudBlobContainer()
        {
            CloudStorageAccount storageAccount =
                CloudStorageAccount.Parse(
                    WebConfigurationManager.AppSettings["collabbathstorage_AzureStorageConnectionString"]);
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference("collabbath-blob-container");
            container.CreateIfNotExists();
            return container;
        }

        #endregion Private Methods
    }
}