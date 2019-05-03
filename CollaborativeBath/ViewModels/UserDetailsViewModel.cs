using CollaborativeBath.Models;
using System.Collections.Generic;

namespace CollaborativeBath.ViewModels
{
    public class UserDetailsViewModel
    {
        public string UserName { get; set; }
        public IList<File> Files { get; set; }
        public int VoteSum { get; set; }
        public string UserId { get; set; }
    }
}