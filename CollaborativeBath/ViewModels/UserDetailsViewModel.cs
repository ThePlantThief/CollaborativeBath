using CollaborativeBath.Models;
using System.Collections.Generic;

namespace CollaborativeBath.ViewModels
{
    /// <summary>
    /// ViewModel contain details for a user.
    /// </summary>
    public class UserDetailsViewModel
    {
        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        /// <value>
        /// The name of the user.
        /// </value>
        public string UserName { get; set; }
        /// <summary>
        /// Gets or sets the files uploaded by the user.
        /// </summary>
        /// <value>
        /// The files.
        /// </value>
        public IList<File> Files { get; set; }
        /// <summary>
        /// Gets or sets the vote sum of the user.
        /// </summary>
        /// <value>
        /// The vote sum.
        /// </value>
        public int VoteSum { get; set; }
        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>
        /// The user identifier.
        /// </value>
        public string UserId { get; set; }
    }
}