using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CollaborativeBath.Models
{
    /// <summary>
    /// Enum of all folder types
    /// </summary>
    public enum FolderType
    {
        /// <summary>
        /// Is a top layer folder
        /// </summary>
        TopLayer = 1,
        /// <summary>
        /// Is a course folder
        /// </summary>
        Course = 2,
        /// <summary>
        /// Is a unit folder
        /// </summary>
        Unit = 3,
        /// <summary>
        /// Not one of the other types
        /// </summary>
        Other = 4,
    }

    /// <summary>
    /// Model representing a folder.
    /// </summary>
    /// <seealso cref="CollaborativeBath.Models.Commentable" />
    public class Folder : Commentable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Folder"/> class.
        /// </summary>
        public Folder()
        {
            Children = new List<Folder>();
            Items = new List<File>();
        }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id { get; set; }
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public string Title { get; set; }
        /// <summary>
        /// Gets or sets the parent folder.
        /// </summary>
        /// <value>
        /// The parent folder. If null then the folder is a
        /// top level folder.
        /// </value>
        public Folder Parent { get; set; }

        /// <summary>
        /// Gets or sets the parent identifier.
        /// </summary>
        /// <value>
        /// The parent identifier. If null then the folder is a
        /// top level folder.
        /// </value>
        [ForeignKey("Parent")]
        public int? ParentId { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public FolderType Type { get; set; }
        /// <summary>
        /// Gets or sets the child folders.
        /// </summary>
        /// <value>
        /// The childen.
        /// </value>
        public IList<Folder> Children { get; set; }
        /// <summary>
        /// Gets or sets the panoptos contained in the folder.
        /// </summary>
        /// <value>
        /// The panoptos.
        /// </value>
        public IList<Panopto> Panoptos { get; set; }
        /// <summary>
        /// Gets or sets the items contained in the folder.
        /// </summary>
        /// <value>
        /// The items.
        /// </value>
        public ICollection<File> Items { get; set; }
        /// <summary>
        /// Gets or sets the subscribed users.
        /// </summary>
        /// <value>
        /// The subscribed users.
        /// </value>
        public ICollection<ApplicationUser> SubscribedUsers { get; set; }
    }

    /// <summary>
    /// Model Representing a file
    /// </summary>
    /// <seealso cref="CollaborativeBath.Models.Commentable" />
    public class File : Commentable
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id { get; set; }
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
        public string Type { get; set; }
        /// <summary>
        /// Gets or sets the parent folder.
        /// </summary>
        /// <value>
        /// The parent folder.
        /// </value>
        public virtual Folder Parent { get; set; }
        /// <summary>
        /// Gets or sets the user.
        /// </summary>
        /// <value>
        /// The user.
        /// </value>
        public virtual ApplicationUser User { get; set; }
        /// <summary>
        /// Gets or sets the parent file (for versioning).
        /// </summary>
        /// <value>
        /// The parent file.
        /// </value>
        public virtual File ParentFile { get; set; }
    }
}