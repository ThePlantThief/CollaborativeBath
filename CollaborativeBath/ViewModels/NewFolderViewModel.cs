using CollaborativeBath.Models;
using System.Collections.Generic;
using System.Web.Mvc;

namespace CollaborativeBath.ViewModels
{
    /// <summary>
    /// View Model for creating a new folder.
    /// </summary>
    public class NewFolderViewModel
    {
        /// <summary>
        /// Gets or sets the parent.
        /// </summary>
        /// <value>
        /// The parent.
        /// </value>
        public Folder Parent { get; set; }
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public string Title { get; set; }
        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public FolderType Type { get; set; }
        /// <summary>
        /// Gets or sets the type list to be displayed in the views dropdown.
        /// </summary>
        /// <value>
        /// The type list.
        /// </value>
        public IEnumerable<SelectListItem> TypeList { get; set; }
    }
}