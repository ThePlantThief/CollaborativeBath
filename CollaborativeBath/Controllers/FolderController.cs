using CollaborativeBath.Models;
using CollaborativeBath.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CollaborativeBath.Controllers
{
    /// <summary>
    /// Controller for the Folder Model.
    /// Handles creation, modification and display.
    /// </summary>
    /// <seealso cref="System.Web.Mvc.Controller" />
    [Authorize]
    public class FolderController : Controller
    {
        //GET: Index
        /// <summary>
        /// Returns a view displaying a list of Top-Level folders that do not have a parent folder.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var topLayer = new Folder();
            topLayer.Title = "All Courses";
            topLayer.Id = -1;
            topLayer.Type = FolderType.TopLayer;
            topLayer.Panoptos = new List<Panopto>();
            ICollection<Folder> subFolders = new List<Folder>();
            using (MaterialContext db = new MaterialContext())
            {
                topLayer.Children = db.Folders.Where(f => f.Type == FolderType.Course).Include(f => f.VoteList.Votes)
                    .ToList();
                var userId = User.Identity.GetUserId();
                var user = db.Users
                    .Include(u => u.SubscribedFolders.Select(f => f.VoteList.Votes))
                    .FirstOrDefault(u => u.Id == userId);
                subFolders = user.SubscribedFolders;
            }

            var leaderboard = UserController.Leaderboard();
            return View("Index", new TopLayerViewModel() { Leaderboard = leaderboard, Folder = topLayer, SubscribedFolders = subFolders });
        }

        // GET: Details
        /// <summary>
        /// Returns a view that displays a list of sub-folders, files, panoptos contained within the folder
        /// and the folders votes and comments.
        /// </summary>
        /// <param name="id">The folder identifier.</param>
        /// <returns></returns>
        public ActionResult Details(int? id)
        {
            Folder folder = null;
            bool subbed = false;
            using (MaterialContext db = new MaterialContext())
            {
                folder = db.Folders
                    .Include(f => f.VoteList.Votes)
                    .Include(f => f.Children.Select(c => c.VoteList.Votes))
                    .Include(f => f.Items.Select(c => c.VoteList.Votes))
                    .Include(f => f.Panoptos.Select(p => p.VoteList.Votes))
                    .Include(f => f.CommentList.Comments.Select(c => c.User))
                    .Include(u => u.CommentList.Comments.Select(c => c.VoteList.Votes))
                    .FirstOrDefault(c => c.Id == id);
                var userId = User.Identity.GetUserId();
                subbed = db.Users
                    .Include(u => u.SubscribedFolders)
                    .FirstOrDefault(f => f.Id == userId)?.SubscribedFolders.Contains(folder) ?? false;
            }

            if (folder != null)
            {
                return View(new FolderDetailsViewModel(folder, GetParents(folder.Id), subbed));
            }
            else
            {
                return null;
            }
        }

        //Get: Create
        /// <summary>
        /// Returns a view to create a new folder.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public ActionResult Create(int? id)
        {
            using (MaterialContext db = new MaterialContext())
            {
                var parent = db.Folders.Include(f => f.Children).FirstOrDefault(f => f.Id == id);
                if (id == -1 || parent != null)
                {
                    if (id == -1)
                    {
                        parent = new Folder()
                        {
                            Title = "Top Level",
                            Type = FolderType.TopLayer,
                            Id = -1
                        };
                    }

                    var selectList = Enum.GetValues(typeof(FolderType))
                        .Cast<FolderType>()
                        .Where(e => e == FolderType.Other || (int)e > (int)parent.Type)
                        .Select(e => new SelectListItem
                        {
                            Value = ((int)e).ToString(),
                            Text = e.ToString()
                        });
                    return View(new NewFolderViewModel() { Parent = parent, TypeList = selectList });
                }
                else
                {
                    return HttpNotFound("Failed to create new folder.");
                }
            }
        }

        //POST: Create
        /// <summary>
        /// Creates a new folder from the specified model.
        /// Redirects to the details page of the new folder.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(NewFolderViewModel model)
        {
            if (!string.IsNullOrWhiteSpace(model.Title))
            {
                Folder folder = new Folder()
                {
                    Title = model.Title,
                    Type = model.Type,
                    VoteList = new VoteList(),
                    CommentList = new CommentList(),
                };
                using (MaterialContext db = new MaterialContext())
                {
                    if (model.Parent.Id != -1)
                    {
                        var parent = db.Folders.Find(model.Parent.Id);
                        parent.Children.Add(folder);
                    }
                    else
                    {
                        db.Folders.Add(folder);
                    }
                    db.SaveChanges();
                }
                NotificationController.NewFolder(folder);
                return RedirectToAction("Details", new { id = folder.Id });
            }
            return RedirectToAction("Create", new { id = model.Parent.Id });
        }

        /// <summary>
        /// (Un)subscribes the requesting user to the folder with
        /// specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="sub">if set to <c>true</c> subscribe, else unsubscribe.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> Subscribe(int id, bool sub)
        {
            using (MaterialContext dbContext = new MaterialContext())
            {
                var userId = User.Identity.GetUserId();
                var user = dbContext.Users.Include(u => u.SubscribedFolders)
                    .FirstOrDefault(u => u.Id == userId);
                var folder = dbContext.Folders.Find(id);
                if (user != null && folder != null)
                {
                    if (sub && !user.SubscribedFolders.Contains(folder))
                    {
                        user.SubscribedFolders.Add(folder);
                    }
                    else if (user.SubscribedFolders.Contains(folder))
                    {
                        user.SubscribedFolders.Remove(folder);
                    }
                }
                dbContext.SaveChanges();
            }

            if (sub)
            {
                return Content("subscribed");
            }
            else
            {
                return Content("unsubscribed");
            }
        }

        /// <summary>
        /// Returns an array of the parent folders of the folder with given
        /// identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
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