using CollaborativeBath.Models;

namespace CollaborativeBath.ViewModels
{
    /// <summary>
    /// View Model for the details page for files
    /// </summary>
    public class FileDetailsViewModel
    {
        /// <summary>
        /// Gets or sets the file.
        /// </summary>
        /// <value>
        /// The file.
        /// </value>
        public File File { get; set; }
        /// <summary>
        /// Gets or sets the breadcrumbs.
        /// </summary>
        /// <value>
        /// The breadcrumbs.
        /// </value>
        public Breadcrumb[] Breadcrumbs { get; set; }

        /// <summary>
        /// Gets or sets the file history IDs.
        /// </summary>
        /// <value>
        /// The history.
        /// </value>
        public int[] History { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileDetailsViewModel"/> class.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="breadcrumbs">The breadcrumbs.</param>
        /// <param name="history">The history.</param>
        public FileDetailsViewModel(File file, Breadcrumb[] breadcrumbs, int[] history)
        {
            File = file;
            Breadcrumbs = breadcrumbs;
            History = history;
        }
    }
}