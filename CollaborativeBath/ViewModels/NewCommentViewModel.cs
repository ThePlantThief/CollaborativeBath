namespace CollaborativeBath.ViewModels.Base
{
    /// <summary>
    /// View Model for creating a new comment.
    /// </summary>
    public class NewCommentViewModel
    {
        /// <summary>
        /// Gets or sets the comment list identifier.
        /// </summary>
        /// <value>
        /// The comment list identifier.
        /// </value>
        public int CommentListId { get; set; }
        /// <summary>
        /// Gets or sets the comment text.
        /// </summary>
        /// <value>
        /// The text.
        /// </value>
        public string Text { get; set; }
    }
}