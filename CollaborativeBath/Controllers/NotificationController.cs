using CollaborativeBath.Models;
using Microsoft.AspNet.Identity;
using PusherServer;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CollaborativeBath.Controllers
{
    public class NotificationController : Controller
    {
        #region Public Methods

        [NonAction]
        public static void AddNotification(Notification notification, string userId)
        {
            using (MaterialContext db = new MaterialContext())
            {
                var user = db.Users.Include(u => u.Notifications).FirstOrDefault(u => u.Id == userId);
                user.Notifications.Add(notification);
                db.SaveChanges();
            }
            var options = new PusherOptions();
            options.Cluster = "eu";
            var pusher = new Pusher("727030", "e4d1d35525db3ed45a7f", "3c03a5bfb47157e0d653", options);
            JsonNotification jsonNotification = ConvertNotification(notification);
            new Task(() => pusher.TriggerAsync("notifcation_channel_" + userId, "new_note_event", jsonNotification).Wait()).Start();
        }

        public static void NewComment(CommentList list)
        {
            using (MaterialContext db = new MaterialContext())
            {
                list = db.CommentList
                    .Include(l => l.SubscribedUsers.Select(u => u.Notifications))
                    .FirstOrDefault(l => l.Id == list.Id);
            }

            if (list != null)
            {
                string title = CommentController.GetParentTitle(list.Id);
                foreach (ApplicationUser user in list.SubscribedUsers)
                {
                    if (!user.Notifications.Any(n =>
                        n.Controller == "Comment" &&
                        n.Action == "ListDetails" &&
                        n.ItemId == list.Id &&
                        n.UserSeen == false
                    ))
                    {
                        AddNotification(new Notification()
                        {
                            Action = "ListDetails",
                            Controller = "Comment",
                            ItemId = list.Id,
                            Text = "New comment(s) on \'" + title + "\'",
                            Time = DateTime.Now,
                            UserSeen = false
                        }, user.Id);
                    }
                }
            }
        }

        public static void NewFile(File file)
        {
            Folder folder = null;
            using (MaterialContext db = new MaterialContext())
            {
                folder = db.Folders
                    .Include(f => f.SubscribedUsers)
                    .FirstOrDefault(f => f.Id == file.Parent.Id);
            }

            if (folder != null)
            {
                foreach (ApplicationUser user in folder.SubscribedUsers)
                {
                    AddNotification(new Notification()
                    {
                        Action = "Details",
                        Controller = "File",
                        ItemId = file.Id,
                        Text = "File \'" + file.Title + "\' added to \'" + folder.Title + "\'",
                        Time = DateTime.Now,
                        UserSeen = false
                    }, user.Id);
                }
            }
        }

        public static void NewFolder(Folder folder)
        {
            Folder parent = null;
            using (MaterialContext db = new MaterialContext())
            {
                parent = db.Folders
                    .Include(f => f.SubscribedUsers)
                    .FirstOrDefault(f => f.Id == folder.Parent.Id);
            }
            if (parent != null)
            {
                foreach (ApplicationUser user in parent.SubscribedUsers)
                {
                    AddNotification(new Notification()
                    {
                        Action = "Details",
                        Controller = "Folder",
                        ItemId = folder.Id,
                        Text = "Subfolder \'" + folder.Title + "\' added to \'" + parent.Title + "\'",
                        Time = DateTime.Now,
                        UserSeen = false
                    }, user.Id);
                }
            }
        }

        public static void NewVote(VoteList list)
        {
            using (MaterialContext db = new MaterialContext())
            {
                list = db.VoteList
                    .Include(l => l.SubscribedUsers.Select(u => u.Notifications))
                    .FirstOrDefault(l => l.Id == list.Id);
            }

            if (list != null)
            {
                string title = VoteController.GetParentTitle(list.Id);
                foreach (ApplicationUser user in list.SubscribedUsers)
                {
                    if (!user.Notifications.Any(n =>
                        n.Controller == "Vote" &&
                        n.Action == "ListDetails" &&
                        n.ItemId == list.Id &&
                        n.UserSeen == false
                    ))
                    {
                        AddNotification(new Notification()
                        {
                            Action = "ListDetails",
                            Controller = "Vote",
                            ItemId = list.Id,
                            Text = "Someone voted on \'" + title + "\'",
                            Time = DateTime.Now,
                            UserSeen = false
                        }, user.Id);
                    }
                }
            }
        }

        public static void UpdatedFile(File file)
        {
            Folder folder = null;
            using (MaterialContext db = new MaterialContext())
            {
                folder = db.Folders
                    .Include(f => f.SubscribedUsers)
                    .FirstOrDefault(f => f.Id == file.Parent.Id);
            }

            if (folder != null)
            {
                foreach (ApplicationUser user in folder.SubscribedUsers)
                {
                    AddNotification(new Notification()
                    {
                        Action = "Details",
                        Controller = "File",
                        ItemId = file.Id,
                        Text = "A new version of \'" + file.Title + "\' has been uploaded",
                        Time = DateTime.Now,
                        UserSeen = false
                    }, user.Id);
                }
            }
        }

        public static void NewPanopto(Panopto panopto)
        {
            Folder folder = null;
            using (MaterialContext db = new MaterialContext())
            {
                folder = db.Folders
                    .Include(f => f.SubscribedUsers)
                    .FirstOrDefault(f => f.Id == panopto.Parent.Id);
            }

            if (folder != null)
            {
                foreach (ApplicationUser user in folder.SubscribedUsers)
                {
                    AddNotification(new Notification()
                    {
                        Action = "Details",
                        Controller = "Panopto",
                        ItemId = panopto.Id,
                        Text = "A new lecture \'" + panopto.Title + "\' was added to \'" + folder.Title + "\'",
                        Time = DateTime.Now,
                        UserSeen = false
                    }, user.Id);
                }
            }
        }

        [HttpPost]
        public ContentResult Delete(int? noteId)
        {
            if (noteId != null)
            {
                using (MaterialContext db = new MaterialContext())
                {
                    var notification = db.Notifications.Find(noteId);
                    if (notification != null)
                        db.Notifications.Remove(notification);
                    db.SaveChanges();
                }
            }

            return Content("ok");
        }

        public ActionResult GetNotifications()
        {
            ICollection<Notification> notifications = null;
            using (MaterialContext db = new MaterialContext())
            {
                string userId = User.Identity.GetUserId();
                ApplicationUser user = db.Users
                    .Include(n => n.Notifications)
                    .FirstOrDefault(l => l.Id == userId);
                if (user != null)
                {
                    notifications = user.Notifications.ToList();
                    IList<JsonNotification> jsonNotifications = new List<JsonNotification>();
                    foreach (var note in notifications.OrderByDescending(n => n.Time))
                    {
                        if (note.UserSeen && DateTime.Now - note.Time > new TimeSpan(0, 30, 0))
                        {
                            db.Notifications.Remove(note);
                        }
                        else
                        {
                            jsonNotifications.Add(ConvertNotification(note));
                        }
                    }
                    return Json(jsonNotifications.ToArray(), JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new JsonNotification[] { }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ContentResult Seen(int? noteId)
        {
            if (noteId != null)
            {
                using (MaterialContext db = new MaterialContext())
                {
                    var notification = db.Notifications.Find(noteId);
                    notification.UserSeen = true;
                    db.SaveChanges();
                }
            }

            return Content("ok");
        }

        #endregion Public Methods

        #region Private Methods

        private static JsonNotification ConvertNotification(Notification notification)
        {
            return new JsonNotification()
            {
                Time = notification.Time.ToShortDateString() + " " + notification.Time.ToShortTimeString(),
                Href = "/" + notification.Controller + "/" + notification.Action + "/" + notification.ItemId,
                Seen = notification.UserSeen,
                Text = notification.Text,
                Id = notification.Id
            };
        }

        #endregion Private Methods

        #region Public Classes

        public class JsonNotification
        {
            #region Public Properties

            public string Href { get; set; }
            public int Id { get; set; }
            public bool Seen { get; set; }
            public string Text { get; set; }
            public string Time { get; set; }

            #endregion Public Properties
        }

        #endregion Public Classes
    }
}