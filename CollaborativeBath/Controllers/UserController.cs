using CollaborativeBath.Models;
using CollaborativeBath.ViewModels;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace CollaborativeBath.Controllers
{
    /// <summary>
    /// Controller for the ApplicationUser Model.
    /// Handles requests to the details page and leaderboards.
    /// </summary>
    /// <seealso cref="System.Web.Mvc.Controller" />
    [Authorize]
    public class UserController : Controller
    {
        /// <summary>
        /// Struct to represent a user's vote count.
        /// </summary>
        public struct UserVotes
        {
            /// <summary>
            /// The user
            /// </summary>
            public ApplicationUser User;
            /// <summary>
            /// Their total votes
            /// </summary>
            public int Votes;
        }

        // GET: Details
        /// <summary>
        /// Returns the view for /User/Details
        /// </summary>
        /// <param name="username">The username of the user to display.</param>
        /// <returns></returns>
        public ActionResult Details(string username)
        {
            ApplicationUser user = null;
            IList<File> files = null;
            int totalVotes = 0;
            using (MaterialContext dbContext = new MaterialContext())
            {
                user = dbContext.Users.FirstOrDefault(u => u.UserName == username);
                if (user == null)
                {
                    return HttpNotFound("User not found");
                }

                files = dbContext.Files
                    .Include(f => f.VoteList.Votes)
                    .Include(f => f.User)
                    .Where(f => f.User.Id == user.Id).ToList();
                totalVotes = GetUserVotes(user);
            }

            return View(new UserDetailsViewModel()
            {
                Files = files,
                UserName = user.UserName,
                VoteSum = totalVotes,
                UserId = user.Id
            });
        }

        //GET: Leaderboard
        /// <summary>
        /// Generates and returns a list of all users in decending vote order.
        /// </summary>
        /// <returns></returns>
        public static IList<UserVotes> Leaderboard()
        {
            IList<UserVotes> users = new List<UserVotes>();
            using (MaterialContext dbContext = new MaterialContext())
            {
                foreach (var user in dbContext.Users.Where(u => true))
                {
                    users.Add(new UserVotes() { User = user, Votes = GetUserVotes(user) });
                }
            }
            users = users.OrderByDescending(u => u.Votes).ToList();
            return users;
        }

        /// <summary>
        /// Gets the given user vote count.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        private static int GetUserVotes(ApplicationUser user)
        {
            int totalVotes = 0;
            using (MaterialContext dbContext = new MaterialContext())
            {
                var files = dbContext.Files
                    .Include(f => f.VoteList.Votes)
                    .Include(f => f.User)
                    .Where(f => f.User.Id == user.Id).ToList();
                var comments = dbContext.Comments
                    .Include(f => f.VoteList.Votes)
                    .Include(f => f.User)
                    .Where(f => f.User.Id == user.Id).ToList();
                foreach (var comment in comments)
                {
                    totalVotes += comment.VoteList.VoteSum;
                }

                foreach (var file in files)
                {
                    totalVotes += file.VoteList.VoteSum;
                }
            }

            return totalVotes;
        }
    }
}