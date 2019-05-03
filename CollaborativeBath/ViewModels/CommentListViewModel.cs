using CollaborativeBath.Models;
using System.Collections.Generic;

namespace CollaborativeBath.ViewModels.Base
{
    public class CommentListViewModel
    {
        public int CommentListId { get; set; }
        public ICollection<Comment> Comments { get; set; }
    }
}