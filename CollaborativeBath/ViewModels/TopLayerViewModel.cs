using CollaborativeBath.Controllers;
using CollaborativeBath.Models;
using System.Collections.Generic;

namespace CollaborativeBath.ViewModels
{
    /// <summary>
    /// View Model for the TopLayer View.
    /// </summary>
    public class TopLayerViewModel
    {
        /// <summary>
        /// Folder object describing the root folders title.
        /// </summary>
        public Folder Folder;
        /// <summary>
        /// The leaderboard of users in desending vote order.
        /// </summary>
        public IList<UserController.UserVotes> Leaderboard;
        /// <summary>
        /// The subscribed folders of the requesting user.
        /// </summary>
        public ICollection<Folder> SubscribedFolders;
    }
}