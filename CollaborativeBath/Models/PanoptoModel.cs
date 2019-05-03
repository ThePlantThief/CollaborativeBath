using System.ComponentModel.DataAnnotations;

namespace CollaborativeBath.Models
{
    public class Panopto : Commentable
    {
        public int Id { get; set; }

        [Display(Name = "Panopto Link")]
        public string PanoptoId { get; set; }

        public ApplicationUser Uploader { get; set; }
        public Folder Parent { get; set; }
        public string Title { get; set; }
    }
}