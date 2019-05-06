using System;
using System.ComponentModel.DataAnnotations;

namespace CollaborativeBath.Models
{
    /// <summary>
    /// Model representing a notification
    /// </summary>
    public class Notification
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the action of the related controller.
        /// </summary>
        /// <value>
        /// The action.
        /// </value>
        public string Action { get; set; }
        /// <summary>
        /// Gets or sets the related controller.
        /// </summary>
        /// <value>
        /// The controller.
        /// </value>
        public string Controller { get; set; }
        /// <summary>
        /// Gets or sets the related item's identifier.
        /// </summary>
        /// <value>
        /// The item identifier.
        /// </value>
        public int ItemId { get; set; }
        /// <summary>
        /// Gets or sets the notification text.
        /// </summary>
        /// <value>
        /// The text.
        /// </value>
        public string Text { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether the user has seen
        /// the notification.
        /// </summary>
        /// <value>
        ///   <c>true</c> if user seen; otherwise, <c>false</c>.
        /// </value>
        public bool UserSeen { get; set; }
        /// <summary>
        /// Gets or sets the time of occurance.
        /// </summary>
        /// <value>
        /// The time.
        /// </value>
        public DateTime Time { get; set; }
    }
}