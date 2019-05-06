using CollaborativeBath.Models;
using Microsoft.AspNet.Identity;
using PusherServer;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CollaborativeBath.Controllers
{
    /// <summary>
    /// Controller for the Vote Model. Handles modification and fetching of vote entities.
    /// </summary>
    /// <seealso cref="System.Web.Mvc.Controller" />
    [Authorize]
    public class VoteController : Controller
    {
        // GET: Create
        /// <summary>
        /// Creates/modifies a vote for the given VoteList for the 
        /// requesting user.
        /// </summary>
        /// <param name="id">The identifier of the VoteList.</param>
        /// <param name="up">True if Up Vote, else Down Vote.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> Create(int? id, bool? up)
        {
            if (id != null && up != null)
            {
                int voteUp = 0;
                int voteDown = 0;
                int commentId = -1;
                bool voteChanged = false;
                VoteList voteList = null;
                using (MaterialContext db = new MaterialContext())
                {
                    ApplicationUser user = db.Users.Find(User.Identity.GetUserId());
                    voteList = db.VoteList
                        .Include(l => l.Votes)
                        .FirstOrDefault(v => v.Id == id);
                    Vote vote = voteList?.Votes.FirstOrDefault(v => v.User == user);
                    if (voteList != null && vote == null)
                    {
                        vote = new Vote
                        {
                            Up = up.Value,
                            User = user,
                        };
                        voteList.Votes.Add(vote);
                        db.SaveChanges();
                        voteChanged = true;
                    }
                    else if (vote != null && vote.Up != up)
                    {
                        vote.Up = up.Value;
                        db.SaveChanges();
                        voteChanged = true;
                    }
                    voteUp = voteList.VoteUp;
                    voteDown = voteList.VoteDown;
                    commentId = db.Comments.Include(c => c.VoteList)
                        .FirstOrDefault(l => l.VoteList.Id == id)?.Id ?? -1;
                }

                if (voteChanged)
                {
                    NotificationController.NewVote(voteList);
                    var options = new PusherOptions();
                    options.Cluster = "eu";
                    var pusher = new Pusher("727030", "e4d1d35525db3ed45a7f", "3c03a5bfb47157e0d653", options);
                    ITriggerResult result = await pusher.TriggerAsync("vote_channel", "vote_update",
                        new { Id = id.Value, UpVotes = voteUp, DownVotes = voteDown });
                    if (commentId != -1)
                    {
                        result = await pusher.TriggerAsync("comment_channel", "vote_update",
                            new { Id = commentId, VoteUp = voteUp, VoteDown = voteDown });
                    }
                }
            }
            return Content("ok");
        }

        /// <summary>
        /// Gets the parent entity of the vote list and returns its
        /// title.
        /// </summary>
        /// <param name="listId">The list identifier.</param>
        /// <returns></returns>
        public static string GetParentTitle(int listId)
        {
            using (MaterialContext db = new MaterialContext())
            {
                //Check folders
                if (db.Folders
                    .Include(f => f.VoteList)
                    .Any(f => f.VoteList.Id == listId))
                {
                    Folder folder = db.Folders
                        .Include(f => f.VoteList)
                        .FirstOrDefault(f => f.VoteList.Id == listId);
                    return folder.Title;
                }
                else if (db.Files
                    .Include(f => f.VoteList)
                    .Any(f => f.VoteList.Id == listId))
                {
                    File file = db.Files
                        .Include(f => f.VoteList)
                        .FirstOrDefault(f => f.VoteList.Id == listId);
                    return file.Title;
                }
                else if (db.Panoptos
                    .Include(f => f.VoteList)
                    .Any(f => f.VoteList.Id == listId))
                {
                    Panopto panopto = db.Panoptos
                        .Include(f => f.VoteList)
                        .FirstOrDefault(f => f.VoteList.Id == listId);
                    return panopto.Title;
                }

                return "";
            }
        }

        /// <summary>
        /// Returns a RedirectToAction to the details page for the parent entity.
        /// If no entity is found returns HttpNotFound.
        /// </summary>
        /// <param name="id">The list identifier.</param>
        /// <returns></returns>
        public ActionResult ListDetails(int? id)
        {
            using (MaterialContext db = new MaterialContext())
            {
                //Check folders
                if (db.Folders
                    .Include(f => f.VoteList)
                    .Any(f => f.VoteList.Id == id))
                {
                    Folder folder = db.Folders
                        .Include(f => f.VoteList)
                        .FirstOrDefault(f => f.VoteList.Id == id);
                    return RedirectToAction("Details", "Folder", new { id = folder.Id });
                }
                else if (db.Files
                    .Include(f => f.VoteList)
                    .Any(f => f.VoteList.Id == id))
                {
                    File file = db.Files
                        .Include(f => f.VoteList)
                        .FirstOrDefault(f => f.VoteList.Id == id);
                    return RedirectToAction("Details", "File", new { id = file.Id });
                }
                else if (db.Panoptos
                    .Include(f => f.VoteList)
                    .Any(f => f.VoteList.Id == id))
                {
                    Panopto panopto = db.Panoptos
                        .Include(f => f.VoteList)
                        .FirstOrDefault(f => f.VoteList.Id == id);
                    return RedirectToAction("Details", "Panopto", new { id = panopto.Id });
                }

                return HttpNotFound("Not Found");
            }
        }
    }
}