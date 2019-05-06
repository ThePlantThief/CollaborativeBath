using System.ComponentModel.DataAnnotations;

namespace CollaborativeBath.Models
{
    /// <summary>
    /// Model representing a panopto.
    /// </summary>
    /// <seealso cref="CollaborativeBath.Models.Commentable" />
    public class Panopto : Commentable
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the panopto id (the id from the UoB Panopto server).
        /// </summary>
        /// <value>
        /// The panopto identifier.
        /// </value>
        [Display(Name = "Panopto Link")]
        public string PanoptoId { get; set; }

        /// <summary>
        /// Gets or sets the uploader.
        /// </summary>
        /// <value>
        /// The uploader.
        /// </value>
        public ApplicationUser Uploader { get; set; }
        /// <summary>
        /// Gets or sets the parent folder.
        /// </summary>
        /// <value>
        /// The parent folder.
        /// </value>
        public Folder Parent { get; set; }
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public string Title { get; set; }
    }
}