using CollaborativeBath.Models;

namespace CollaborativeBath.ViewModels
{
    /// <summary>
    /// View Model for the details view for a folder
    /// </summary>
    public class FolderDetailsViewModel
    {
        /// <summary>
        /// Gets or sets the folder.
        /// </summary>
        /// <value>
        /// The folder.
        /// </value>
        public Folder Folder { get; set; }
        /// <summary>
        /// Gets or sets the breadcrumbs.
        /// </summary>
        /// <value>
        /// The breadcrumbs.
        /// </value>
        public Breadcrumb[] Breadcrumbs { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user is subscribed to the folder.
        /// </summary>
        /// <value>
        ///   <c>true</c> if subscribed; otherwise, <c>false</c>.
        /// </value>
        public bool Subscribed { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FolderDetailsViewModel"/> class.
        /// </summary>
        /// <param name="folder">The folder.</param>
        /// <param name="breadcrumbs">The breadcrumbs.</param>
        /// <param name="subbed">if set to <c>true</c> [subbed].</param>
        public FolderDetailsViewModel(Folder folder, Breadcrumb[] breadcrumbs, bool subbed)
        {
            Folder = folder;
            Breadcrumbs = breadcrumbs;
            Subscribed = subbed;
        }
    }
}