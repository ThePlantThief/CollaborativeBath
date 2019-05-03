using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CollaborativeBath.Models
{
    public enum FolderType
    {
        TopLayer = 1,
        Course = 2,
        Unit = 3,
        Other = 4,
    }

    public class Folder : Commentable
    {
        public Folder()
        {
            Childen = new List<Folder>();
            Items = new List<File>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public Folder Parent { get; set; }

        [ForeignKey("Parent")]
        public int? ParentId { get; set; }

        public FolderType Type { get; set; }
        public IList<Folder> Childen { get; set; }
        public IList<Panopto> Panoptos { get; set; }
        public ICollection<File> Items { get; set; }
        public ICollection<ApplicationUser> SubscribedUsers { get; set; }
    }

    public class File : Commentable
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public virtual Folder Parent { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual File ParentFile { get; set; }
    }
}