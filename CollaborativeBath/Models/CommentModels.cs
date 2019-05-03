using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CollaborativeBath.Models
{
    public abstract class Commentable : Voteable
    {
        [Required]
        public CommentList CommentList { get; set; }
    }

    public class CommentList
    {
        public int Id { get; set; }

        public CommentList()
        {
            Comments = new List<Comment>();
        }

        public ICollection<Comment> Comments { get; set; }
        public ICollection<ApplicationUser> SubscribedUsers { get; set; } = new List<ApplicationUser>();
    }

    public class Comment : Voteable
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public virtual ApplicationUser User { get; set; }
        public DateTime Time { get; set; }
    }
}