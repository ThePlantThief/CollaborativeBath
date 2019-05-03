using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace CollaborativeBath.Models
{
    public class MaterialContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Comment> Comments { get; set; }

        public DbSet<Vote> Votes { get; set; }

        public DbSet<VoteList> VoteList { get; set; }

        public DbSet<CommentList> CommentList { get; set; }

        public DbSet<Folder> Folders { get; set; }

        public DbSet<File> Files { get; set; }

        public DbSet<Panopto> Panoptos { get; set; }

        public DbSet<Notification> Notifications { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MaterialContext"/> class.
        /// </summary>
        public MaterialContext() : base("DbConnection")
        {
            Database.SetInitializer<MaterialContext>(null);
            Configuration.ProxyCreationEnabled = true;
            Configuration.LazyLoadingEnabled = false;
        }

        public static MaterialContext Create()
        {
            return new MaterialContext();
        }

        /// <summary>
        /// This method is called when the model for a derived context has been initialized, but
        /// before the model has been locked down and used to initialize the context.  The default
        /// implementation of this method does nothing, but it can be overridden in a derived class
        /// such that the model can be further configured before it is locked down.
        /// </summary>
        /// <param name="modelBuilder">The builder that defines the model for the context being created.</param>
        /// <remarks>
        /// Typically, this method is called only once when the first instance of a derived context
        /// is created.  The model for that context is then cached and is for all further instances of
        /// the context in the app domain.  This caching can be disabled by setting the ModelCaching
        /// property on the given ModelBuidler, but note that this can seriously degrade performance.
        /// More control over caching is provided through use of the DbModelBuilder and DbContextFactory
        /// classes directly.
        /// </remarks>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Folder>().HasMany(f => f.Childen)
                .WithOptional()
                .HasForeignKey(f => f.ParentId);
            modelBuilder.Entity<CommentList>().HasMany<ApplicationUser>(l => l.SubscribedUsers)
                .WithMany(u => u.SubscribedCommentLists);
            modelBuilder.Entity<VoteList>().HasMany<ApplicationUser>(l => l.SubscribedUsers)
                .WithMany(u => u.SubscribedVotelists);
        }
    }
}