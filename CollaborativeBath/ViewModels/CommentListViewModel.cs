using CollaborativeBath.Models;
using System.Collections.Generic;

namespace CollaborativeBath.ViewModels.Base
{
    /// <summary>
    /// View Model for a List of comments.
    /// </summary>
    public class CommentListViewModel
    {
        /// <summary>
        /// Gets or sets the comment list identifier.
        /// </summary>
        /// <value>
        /// The comment list identifier.
        /// </value>
        public int CommentListId { get; set; }
        /// <summary>
        /// Gets or sets the comments.
        /// </summary>
        /// <value>
        /// The comments.
        /// </value>
        public ICollection<Comment> Comments { get; set; }
    }
}