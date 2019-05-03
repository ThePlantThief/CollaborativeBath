using CollaborativeBath.Models;
using CollaborativeBath.ViewModels;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace CollaborativeBath.Controllers
{
    [Authorize]
    public class PanoptoController : Controller
    {
        // GET: Panopto
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return HttpNotFound("Video Not Found");
            }

            Panopto panopto = null;
            using (MaterialContext dbContext = new MaterialContext())
            {
                panopto = dbContext.Panoptos
                    .Include(p => p.Uploader)
                    .Include(p => p.Parent)
                    .Include(p => p.CommentList.Comments.Select(c => c.User))
                    .Include(p => p.CommentList.Comments.Select(c => c.VoteList.Votes))
                    .Include(p => p.VoteList.Votes)
                    .FirstOrDefault(p => p.Id == id);
            }

            return View(new PanoptoDetailsViewModel(panopto, GetParents(panopto.Parent.Id)));
        }

        public ActionResult Create(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            return View(new PanoptoUploadViewModel() { ParentId = id.Value, Title = "Panopto - " });
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PanoptoUploadViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return null;
            }

            if (!model.PanoptoId.StartsWith("https"))
            {
                return RedirectToAction("Create");
            }
            else
            {
                string id = model.PanoptoId.Substring(model.PanoptoId.LastIndexOf("id=") + 3);
                model.PanoptoId = id;
            }
            Panopto panopto = null;
            using (MaterialContext db = new MaterialContext())
            {
                ApplicationUser user = db.Users.Find(User.Identity.GetUserId());
                Folder folder = db.Folders.Find(model.ParentId);
                panopto = new Panopto()
                {
                    Title = model.Title,
                    PanoptoId = model.PanoptoId,
                    Uploader = user,
                    Parent = folder,
                    VoteList = new VoteList(),
                    CommentList = new CommentList()
                };
                panopto.VoteList.SubscribedUsers.Add(user);
                panopto.CommentList.SubscribedUsers.Add(user);
                db.Panoptos.Add(panopto);
                db.SaveChanges();
            }
            NotificationController.NewPanopto(panopto);
            return RedirectToAction("Details", "Panopto", new { id = panopto.Id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int? id)
        {
            if (ModelState.IsValid)
            {
                int parentId = 0;
                using (MaterialContext db = new MaterialContext())
                {
                    ApplicationUser user = db.Users.Find(User.Identity.GetUserId());
                    Panopto panopto = db.Panoptos
                        .Include(c => c.VoteList.Votes)
                        .Include(c => c.Parent)
                        .Include(c => c.CommentList.Comments.Select(l => l.VoteList.Votes))
                        .FirstOrDefault(c => c.Id == id);
                    if (panopto != null && panopto.Uploader == user)
                    {
                        db.Panoptos.Remove(panopto);
                        db.SaveChanges();
                    }

                    parentId = panopto.Parent.Id;
                    db.SaveChanges();
                    return RedirectToAction("Details", "Folder", new { id = parentId });
                }
            }
            return Redirect(Request.UrlReferrer.ToString());
        }

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
    }
}