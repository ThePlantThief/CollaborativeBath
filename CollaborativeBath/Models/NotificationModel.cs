using System;
using System.ComponentModel.DataAnnotations;

namespace CollaborativeBath.Models
{
    public class Notification
    {
        [Key]
        public int Id { get; set; }

        public string Action { get; set; }
        public string Controller { get; set; }
        public int ItemId { get; set; }
        public string Text { get; set; }
        public bool UserSeen { get; set; }
        public DateTime Time { get; set; }
    }
}