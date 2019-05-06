namespace CollaborativeBath.ViewModels
{
    /// <summary>
    /// Viewl Model for uploading a Panopto
    /// </summary>
    public class PanoptoUploadViewModel
    {
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public string Title { get; set; }
        /// <summary>
        /// Gets or sets the panopto identifier.
        /// </summary>
        /// <value>
        /// The panopto identifier.
        /// </value>
        public string PanoptoId { get; set; }
        /// <summary>
        /// Gets or sets the parent folder identifier.
        /// </summary>
        /// <value>
        /// The parent identifier.
        /// </value>
        public int ParentId { get; set; }
    }
}