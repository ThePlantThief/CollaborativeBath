using CollaborativeBath.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace CollaborativeBath.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    using System.Linq;

    /// <summary>
    /// Configuration class for EntityFramework migration and seeding
    /// </summary>
    /// <seealso cref="System.Data.Entity.Migrations.DbMigrationsConfiguration{CollaborativeBath.Models.MaterialContext}" />
    internal sealed class Configuration : DbMigrationsConfiguration<CollaborativeBath.Models.MaterialContext>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Configuration"/> class.
        /// </summary>
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        /// <summary>
        /// Runs after upgrading to the latest migration to allow seed data to be updated.
        /// </summary>
        /// <param name="context">Context to be used for updating seed data.</param>
        /// <remarks>
        /// Note that the database may already contain seed data when this method runs. This means that
        /// implementations of this method must check whether or not seed data is present and/or up-to-date
        /// and then only make changes if necessary and in a non-destructive way. The
        /// <see cref="M:System.Data.Entity.Migrations.DbSetMigrationsExtensions.AddOrUpdate``1(System.Data.Entity.IDbSet{``0},``0[])" />
        /// can be used to help with this, but for seeding large amounts of data it may be necessary to do less
        /// granular checks if performance is an issue.
        /// If the <see cref="T:System.Data.Entity.MigrateDatabaseToLatestVersion`2" /> database
        /// initializer is being used, then this method will be called each time that the initializer runs.
        /// If one of the <see cref="T:System.Data.Entity.DropCreateDatabaseAlways`1" />, <see cref="T:System.Data.Entity.DropCreateDatabaseIfModelChanges`1" />,
        /// or <see cref="T:System.Data.Entity.CreateDatabaseIfNotExists`1" /> initializers is being used, then this method will not be
        /// called and the Seed method defined in the initializer should be used instead.
        /// </remarks>
        protected override void Seed(CollaborativeBath.Models.MaterialContext context)
        {
            System.Diagnostics.Debugger.Launch();

            //Users
            var admin = context.Users.FirstOrDefault(u => u.UserName == "Admin");
            byte[] pic = System.IO.File.ReadAllBytes(@"C:\Users\08and\source\repos\CollaborativeBath\CollaborativeBath\img\Admin.png");
            if (admin == null)
            {
                admin = new ApplicationUser()
                {
                    UserName = "Admin",
                    Email = "Admin@Admin.com",
                    ProfileImage = pic
                };
                var store = new UserStore<ApplicationUser>(context);
                var manager = new UserManager<ApplicationUser>(store);
                manager.Create(admin, "AdminPassword!");
                context.SaveChanges();
                admin = context.Users.FirstOrDefault(u => u.UserName == "Admin");
            };

            var user1 = context.Users.FirstOrDefault(u => u.UserName == "BobJones");
            if (user1 == null)
            {
                user1 = new ApplicationUser()
                {
                    UserName = "BobJones",
                    Email = "BobJones@Admin.com",
                    ProfileImage = pic
                };
                var store = new UserStore<ApplicationUser>(context);
                var manager = new UserManager<ApplicationUser>(store);
                manager.Create(user1, "AdminPassword!");
                context.SaveChanges();
                user1 = context.Users.FirstOrDefault(u => u.UserName == "BobJones");
            };

            var user2 = context.Users.FirstOrDefault(u => u.UserName == "SteveJobs");
            if (user2 == null)
            {
                user2 = new ApplicationUser()
                {
                    UserName = "SteveJobs",
                    Email = "SteveJobs@Admin.com",
                    ProfileImage = pic
                };
                var store = new UserStore<ApplicationUser>(context);
                var manager = new UserManager<ApplicationUser>(store);
                manager.Create(user2, "AdminPassword!");
                context.SaveChanges();
                user2 = context.Users.FirstOrDefault(u => u.UserName == "SteveJobs");
            };

            var compsci = context.Folders.FirstOrDefault(f => f.Title.Equals("Computer Science"));
            if (compsci == null)
            {
                compsci = new Folder()
                {
                    Title = "Computer Science",
                    Type = Models.FolderType.Course,
                    CommentList = new CommentList(),
                    VoteList = new VoteList()
                };
                context.Folders.AddOrUpdate(compsci);
            }

            //Units
            var crypto = context.Folders.FirstOrDefault(f => f.Title.Equals("Crypto"));
            if (crypto == null)
            {
                crypto = new Folder()
                {
                    Title = "Crypto",
                    Type = Models.FolderType.Unit,
                    CommentList = new CommentList(),
                    VoteList = new VoteList(),
                    Parent = compsci
                };
                context.Folders.AddOrUpdate(crypto);
            }

            //Other
            var notes = context.Folders.FirstOrDefault(f => f.Title.Equals("Revision Notes"));
            if (notes == null)
            {
                notes = new Folder()
                {
                    Title = "Revision Notes",
                    Type = Models.FolderType.Other,
                    CommentList = new CommentList(),
                    VoteList = new VoteList(),
                    Parent = crypto
                }; context.Folders.AddOrUpdate(notes);
            }

            var past = context.Folders.FirstOrDefault(f => f.Title.Equals("Revision Notes"));
            if (past == null)
            {
                past = new Folder()
                {
                    Title = "Past year's content",
                    Type = Models.FolderType.Other,
                    CommentList = new CommentList(),
                    VoteList = new VoteList(),
                    Parent = crypto
                };
                context.Folders.AddOrUpdate(past);
            }

            //Lectures
            var crypto1 = context.Panoptos.FirstOrDefault(p => p.Title == "Lecture 1");
            if (crypto1 == null)
            {
                crypto1 = new Models.Panopto()
                {
                    PanoptoId = "7478858f-5f62-4c30-9f15-a9f200fbd010",
                    Uploader = user1,
                    Parent = crypto,
                    Title = "Lecture 1",
                    CommentList = new CommentList(),
                    VoteList = new VoteList()
                }; context.Panoptos.AddOrUpdate(crypto1);
            }

            var crypto2 = context.Panoptos.FirstOrDefault(p => p.Title == "Lecture 2");
            if (crypto2 == null)
            {
                crypto2 = new Models.Panopto()
                {
                    PanoptoId = "f306151b-ea8b-4472-a362-a9f5009910d9",
                    Uploader = user1,
                    Parent = crypto,
                    Title = "Lecture 2",
                    CommentList = new CommentList(),
                    VoteList = new VoteList()
                }; context.Panoptos.AddOrUpdate(crypto2);
            }

            var crypto3 = context.Panoptos.FirstOrDefault(p => p.Title == "Lecture 3");
            if (crypto3 == null)
            {
                crypto3 = new Models.Panopto()
                {
                    PanoptoId = "6cf5427f-7bbb-46f6-82e7-a9f400888566",
                    Uploader = user1,
                    Parent = crypto,
                    Title = "Lecture 3",
                    CommentList = new CommentList(),
                    VoteList = new VoteList()
                }; context.Panoptos.AddOrUpdate(crypto3);
            }

            //Comments
            //User1 crypto lecture1
            string[] comments1 = new string[]
            {
                "In this lecture he doesn't really cover anything till @24:39",
                "I found this helpful article about the subsitution encryption method he talks about at @42:12 " + @"https://en.wikipedia.org/wiki/Cryptography"
            };
            foreach (var comment in comments1)
            {
                var comment1 = context.Comments.FirstOrDefault(c => c.Text == comment);
                if (comment1 == null)
                {
                    comment1 = new Comment()
                    {
                        Text = comment,
                        User = user1,
                        VoteList = new VoteList(),
                        Time = DateTime.Now,
                    };
                    crypto1.CommentList.Comments.Add(comment1);
                }
            }
            //User2 crypto lecture1
            string[] comments2 = new string[]
            {
                "Can someone explain the subsitution method he talks about?",
                "Does anyone know if there's coursework for this module?"
            };
            foreach (var comment in comments2)
            {
                var comment1 = context.Comments.FirstOrDefault(c => c.Text == comment);
                if (comment1 == null)
                {
                    comment1 = new Comment()
                    {
                        Text = comment,
                        User = user2,
                        VoteList = new VoteList(),
                        Time = DateTime.Now,
                    };
                    crypto1.CommentList.Comments.Add(comment1);
                }
            }

            //admin past comment
            string[] comments3 = new string[]
            {
                "I took this course last year so if anyone wants any notes on anything I'll upload them in here" +
                " and feel free to comment in here to request extra stuff. I will happily upload it if I have it!"
            };
            foreach (var comment in comments3)
            {
                var comment1 = context.Comments.FirstOrDefault(c => c.Text == comment);
                if (comment1 == null)
                {
                    comment1 = new Comment()
                    {
                        Text = comment,
                        User = admin,
                        VoteList = new VoteList(),
                        Time = DateTime.Now,
                    };
                    past.CommentList.Comments.Add(comment1);
                }
            }

            //Save
            //context.Folders.AddOrUpdate(maths);
            context.SaveChanges();
        }
    }
}