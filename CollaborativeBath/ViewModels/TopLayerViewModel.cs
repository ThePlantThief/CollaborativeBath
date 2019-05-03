using CollaborativeBath.Controllers;
using CollaborativeBath.Models;
using System.Collections.Generic;

namespace CollaborativeBath.ViewModels
{
    public class TopLayerViewModel
    {
        public Folder Folder;
        public IList<UserController.UserVotes> Leaderboard;
        public ICollection<Folder> SubscribedFolders;
    }
}