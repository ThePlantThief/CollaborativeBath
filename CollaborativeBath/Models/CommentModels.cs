using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CollaborativeBath.Models
{
    /// <summary>
    /// Abstract class for models that can be commented on.
    /// </summary>
    /// <seealso cref="CollaborativeBath.Models.Voteable" />
    public abstract class Commentable : Voteable
    {
        /// <summary>
        /// Gets or sets the comment list.
        /// </summary>
        /// <value>
        /// The comment list.
        /// </value>
        [Required]
        public CommentList CommentList { get; set; }
    }

    /// <summary>
    /// A list of commets
    /// </summary>
    public class CommentList
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommentList"/> class.
        /// </summary>
        public CommentList()
        {
            Comments = new List<Comment>();
        }

        /// <summary>
        /// Gets or sets the comments.
        /// </summary>
        /// <value>
        /// The comments.
        /// </value>
        public ICollection<Comment> Comments { get; set; }
        /// <summary>
        /// Gets or sets the subscribed users.
        /// </summary>
        /// <value>
        /// The subscribed users.
        /// </value>
        public ICollection<ApplicationUser> SubscribedUsers { get; set; } = new List<ApplicationUser>();
    }

    /// <summary>
    /// Model representing a comment.
    /// </summary>
    /// <seealso cref="CollaborativeBath.Models.Voteable" />
    public class Comment : Voteable
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id { get; set; }
        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>
        /// The text.
        /// </value>
        public string Text { get; set; }
        /// <summary>
        /// Gets or sets the owning user.
        /// </summary>
        /// <value>
        /// The user.
        /// </value>
        public virtual ApplicationUser User { get; set; }
        /// <summary>
        /// Gets or sets the time of creation.
        /// </summary>
        /// <value>
        /// The time.
        /// </value>
        public DateTime Time { get; set; }
    }
}