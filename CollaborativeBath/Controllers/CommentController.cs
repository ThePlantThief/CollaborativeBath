﻿using CollaborativeBath.Models;
using Microsoft.AspNet.Identity;
using PusherServer;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CollaborativeBath.Controllers
{
    /// <summary>
    /// Controller for the Comment Model.
    /// Handles creation, modification and deleting.
    /// </summary>
    /// <seealso cref="System.Web.Mvc.Controller" />
    [Authorize]
    public class CommentController : Controller
    {
        /// <summary>
        /// A simplified comment class to be used in Json returns.
        /// </summary>
        public class JsonComment
        {
            /// <summary>
            /// Gets or sets the vote up.
            /// </summary>
            /// <value>
            /// The vote up.
            /// </value>
            public int VoteUp { get; set; }
            /// <summary>
            /// Gets or sets the vote down.
            /// </summary>
            /// <value>
            /// The vote down.
            /// </value>
            public int VoteDown { get; set; }
            /// <summary>
            /// Gets or sets the name of the user.
            /// </summary>
            /// <value>
            /// The name of the user.
            /// </value>
            public string UserName { get; set; }
            /// <summary>
            /// Gets or sets the identifier.
            /// </summary>
            /// <value>
            /// The identifier.
            /// </value>
            public int Id { get; set; }
            /// <summary>
            /// Gets or sets the body.
            /// </summary>
            /// <value>
            /// The body.
            /// </value>
            public string Body { get; set; }
            /// <summary>
            /// Gets or sets the time.
            /// </summary>
            /// <value>
            /// The time.
            /// </value>
            public string Time { get; set; }
            /// <summary>
            /// Gets or sets the delete.
            /// </summary>
            /// <value>
            /// The delete.
            /// </value>
            public string Delete { get; set; }
            /// <summary>
            /// Gets or sets the comment list identifier.
            /// </summary>
            /// <value>
            /// The comment list identifier.
            /// </value>
            public int CommentListId { get; set; }
            /// <summary>
            /// Gets or sets the vote list identifier.
            /// </summary>
            /// <value>
            /// The vote identifier.
            /// </value>
            public int VoteId { get; set; }
        }

        /// <summary>
        /// Returns a Json containing an array of all the comments contained
        /// in the comment list with given identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public ActionResult GetComments(int? id)
        {
            CommentList comments = null;
            using (MaterialContext db = new MaterialContext())
            {
                comments = db.CommentList
                    .Include(l => l.Comments.Select(c => c.VoteList.Votes))
                    .Include(l => l.Comments.Select(c => c.User))
                    .FirstOrDefault(l => l.Id == id);
            }

            IList<JsonComment> jsonComments = new List<JsonComment>();
            foreach (var comment in comments.Comments.OrderByDescending(c => c.VoteList.VoteSum))
            {
                jsonComments.Add(ConvertComment(comment));
            }

            return Json(jsonComments.ToArray(), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Creates the comment specified by the JsonComment and adds it to the database.
        /// Pushes the new comment to users.
        /// </summary>
        /// <param name="comment">The comment.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> Create(JsonComment comment)
        {
            if (!string.IsNullOrWhiteSpace(comment.Body))
            {
                CommentList commentList = null;
                Comment newComment = null;
                using (MaterialContext db = new MaterialContext())
                {
                    comment.Body = comment.Body
                        .Replace("<", "&lt;")
                        .Replace(">", "&gt;");
                    ApplicationUser user = db.Users.Find(User.Identity.GetUserId());
                    commentList = db.CommentList
                        .Include(l => l.SubscribedUsers).FirstOrDefault(l => l.Id == comment.CommentListId);
                    newComment = new Comment()
                    {
                        Text = comment.Body,
                        User = user,
                        VoteList = new VoteList(),
                        Time = DateTime.Now
                    };
                    commentList?.Comments.Add(newComment);
                    if (!commentList.SubscribedUsers.Any(u => u.Id == user.Id))
                    {
                        commentList.SubscribedUsers.Add(user);
                    }
                    db.SaveChanges();
                }
                NotificationController.NewComment(commentList);
                var options = new PusherOptions();
                options.Cluster = "eu";
                var pusher = new Pusher("727030", "e4d1d35525db3ed45a7f", "3c03a5bfb47157e0d653", options);
                JsonComment jsonComment = ConvertComment(newComment);
                jsonComment.CommentListId = comment.CommentListId;
                ITriggerResult result = await pusher.TriggerAsync("comment_channel", "new_comment_event", jsonComment);
            }
            return Content("ok");
        }

        // GET: Comment
        /// <summary>
        /// Deletes the comment with specified identifier.
        /// Pushes the deletion event to users.
        /// </summary>
        /// <param name="commentId">The comment identifier.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> Delete(int? commentId)
        {
            if (ModelState.IsValid)
            {
                using (MaterialContext db = new MaterialContext())
                {
                    ApplicationUser user = db.Users.Find(User.Identity.GetUserId());
                    var comment = db.Comments.Include(c => c.VoteList.Votes).FirstOrDefault(c => c.Id == commentId);
                    if (comment != null && comment.User == user)
                    {
                        db.Comments.Remove(comment);
                        var options = new PusherOptions();
                        options.Cluster = "eu";
                        var pusher = new Pusher("727030", "e4d1d35525db3ed45a7f", "3c03a5bfb47157e0d653", options);
                        ITriggerResult result =
                            await pusher.TriggerAsync("comment_channel", "delete_comment_event", commentId);
                        db.SaveChanges();
                    }
                }
            }

            return Content("deleted");
        }

        /// <summary>
        /// Formats the given text to include hyperlinks for timestamp tags.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        private string FormatText(string text)
        {
            string output = "<p>";
            int currentIndex = 0;
            MatchCollection matches = Regex.Matches(text, @"\@[0-9]+?:[0-9]{1,2}"); //(\s|\n)
            foreach (Match match in matches)
            {
                output += text.Substring(currentIndex, match.Index - currentIndex);
                int colonIndex = match.Value.IndexOf(':');
                var minutes = match.Value.Substring(1, colonIndex - 1);
                int time = int.Parse(minutes) * 60;
                time += int.Parse(match.Value.Substring(colonIndex + 1));
                output += "<a href=\"#\" onclick=\"setTime(" + time + ")\">" + (int)(time / 60) + ":" + (time % 60) +
                          "</a>";
                currentIndex = match.Index + match.Length;
            }

            output += text.Substring(currentIndex) + "</p>";
            return output;
        }

        /// <summary>
        /// Converts the comment into a JsonComment.
        /// </summary>
        /// <param name="comment">The comment.</param>
        /// <returns></returns>
        private JsonComment ConvertComment(Comment comment)
        {
            return new JsonComment()
            {
                VoteUp = comment.VoteList.VoteUp,
                VoteDown = comment.VoteList.VoteDown,
                Body = FormatText(comment.Text),
                Id = comment.Id,
                Time = comment.Time.ToShortDateString() + " - " + comment.Time.ToShortTimeString(),
                UserName = comment.User.UserName,
                Delete = comment.User.Id == User.Identity.GetUserId()
                    ? "<button type=\"submit\" style=\"background-color: transparent;" +
                      "border: none\" class=\"oi oi-trash\" onclick=\"delete_comment(" + comment.Id + ")\"></button>"
                    : "",
                VoteId = comment.VoteList.Id
            };
        }

        /// <summary>
        /// Gets the title of the parent item.
        /// </summary>
        /// <param name="listId">The list identifier.</param>
        /// <returns></returns>
        public static string GetParentTitle(int listId)
        {
            using (MaterialContext db = new MaterialContext())
            {
                //Check folders
                if (db.Folders
                    .Include(f => f.CommentList)
                    .Any(f => f.CommentList.Id == listId))
                {
                    Folder folder = db.Folders
                        .Include(f => f.CommentList)
                        .FirstOrDefault(f => f.CommentList.Id == listId);
                    return folder.Title;
                }
                else if (db.Files
                    .Include(f => f.CommentList)
                    .Any(f => f.CommentList.Id == listId))
                {
                    File file = db.Files
                        .Include(f => f.CommentList)
                        .FirstOrDefault(f => f.CommentList.Id == listId);
                    return file.Title;
                }
                else if (db.Panoptos
                    .Include(f => f.CommentList)
                    .Any(f => f.CommentList.Id == listId))
                {
                    Panopto panopto = db.Panoptos
                        .Include(f => f.CommentList)
                        .FirstOrDefault(f => f.CommentList.Id == listId);
                    return panopto.Title;
                }

                return "";
            }
        }

        /// <summary>
        /// Returns a RedirectToAction to the details page of the comment list specified by
        /// the identifier's parent item.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public ActionResult ListDetails(int? id)
        {
            using (MaterialContext db = new MaterialContext())
            {
                //Check folders
                if (db.Folders
                    .Include(f => f.CommentList)
                    .Any(f => f.CommentList.Id == id))
                {
                    Folder folder = db.Folders
                        .Include(f => f.CommentList)
                        .FirstOrDefault(f => f.CommentList.Id == id);
                    return RedirectToAction("Details", "Folder", new { id = folder.Id });
                }
                else if (db.Files
                    .Include(f => f.CommentList)
                    .Any(f => f.CommentList.Id == id))
                {
                    File file = db.Files
                        .Include(f => f.CommentList)
                        .FirstOrDefault(f => f.CommentList.Id == id);
                    return RedirectToAction("Details", "File", new { id = file.Id });
                }
                else if (db.Panoptos
                    .Include(f => f.CommentList)
                    .Any(f => f.CommentList.Id == id))
                {
                    Panopto panopto = db.Panoptos
                        .Include(f => f.CommentList)
                        .FirstOrDefault(f => f.CommentList.Id == id);
                    return RedirectToAction("Details", "Panopto", new { id = panopto.Id });
                }

                return HttpNotFound("Not Found");
            }
        }
    }
}