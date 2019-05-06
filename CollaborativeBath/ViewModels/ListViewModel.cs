using System.Collections.Generic;

namespace CollaborativeBath.ViewModels
{
    /// <summary>
    /// View Model for a list of folders.
    /// </summary>
    public class ListViewModel
    {
        /// <summary>
        /// Gets or sets the list.
        /// </summary>
        /// <value>
        /// The list.
        /// </value>
        public ICollection<CollaborativeBath.Models.Folder> List { get; set; }
        /// <summary>
        /// Gets or sets the name of the controller.
        /// </summary>
        /// <value>
        /// The name of the controller.
        /// </value>
        public string ControllerName { get; set; }
        /// <summary>
        /// Gets or sets the name of the action.
        /// </summary>
        /// <value>
        /// The name of the action.
        /// </value>
        public string ActionName { get; set; }
    }
}