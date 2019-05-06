using CollaborativeBath.Models;

namespace CollaborativeBath.ViewModels
{
    /// <summary>
    /// ViewModel for the details view for a Panopto
    /// </summary>
    public class PanoptoDetailsViewModel
    {
        /// <summary>
        /// Gets or sets the panopto.
        /// </summary>
        /// <value>
        /// The panopto.
        /// </value>
        public Panopto Panopto { get; set; }
        /// <summary>
        /// Gets or sets the breadcrumbs.
        /// </summary>
        /// <value>
        /// The breadcrumbs.
        /// </value>
        public Breadcrumb[] Breadcrumbs { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PanoptoDetailsViewModel"/> class.
        /// </summary>
        /// <param name="panopto">The panopto.</param>
        /// <param name="breadcrumbs">The breadcrumbs.</param>
        public PanoptoDetailsViewModel(Panopto panopto, Breadcrumb[] breadcrumbs)
        {
            Panopto = panopto;
            Breadcrumbs = breadcrumbs;
        }
    }
}