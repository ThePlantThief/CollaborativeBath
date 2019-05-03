namespace CollaborativeBath.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Notifications : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CommentLists",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.Comments",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Text = c.String(),
                    Time = c.DateTime(nullable: false),
                    User_Id = c.String(maxLength: 128),
                    VoteList_Id = c.Int(nullable: false),
                    CommentList_Id = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .ForeignKey("dbo.VoteLists", t => t.VoteList_Id, cascadeDelete: true)
                .ForeignKey("dbo.CommentLists", t => t.CommentList_Id)
                .Index(t => t.User_Id)
                .Index(t => t.VoteList_Id)
                .Index(t => t.CommentList_Id);

            CreateTable(
                "dbo.AspNetUsers",
                c => new
                {
                    Id = c.String(nullable: false, maxLength: 128),
                    ProfileImage = c.Binary(),
                    Email = c.String(maxLength: 256),
                    EmailConfirmed = c.Boolean(nullable: false),
                    PasswordHash = c.String(),
                    SecurityStamp = c.String(),
                    PhoneNumber = c.String(),
                    PhoneNumberConfirmed = c.Boolean(nullable: false),
                    TwoFactorEnabled = c.Boolean(nullable: false),
                    LockoutEndDateUtc = c.DateTime(),
                    LockoutEnabled = c.Boolean(nullable: false),
                    AccessFailedCount = c.Int(nullable: false),
                    UserName = c.String(nullable: false, maxLength: 256),
                })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");

            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    UserId = c.String(nullable: false, maxLength: 128),
                    ClaimType = c.String(),
                    ClaimValue = c.String(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);

            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                {
                    LoginProvider = c.String(nullable: false, maxLength: 128),
                    ProviderKey = c.String(nullable: false, maxLength: 128),
                    UserId = c.String(nullable: false, maxLength: 128),
                })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);

            CreateTable(
                "dbo.Notifications",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Action = c.String(),
                    Controller = c.String(),
                    ItemId = c.Int(nullable: false),
                    Text = c.String(),
                    UserSeen = c.Boolean(nullable: false),
                    Time = c.DateTime(nullable: false),
                    User_Id = c.String(maxLength: 128),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => t.User_Id);

            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                {
                    UserId = c.String(nullable: false, maxLength: 128),
                    RoleId = c.String(nullable: false, maxLength: 128),
                })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);

            CreateTable(
                "dbo.Folders",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Title = c.String(),
                    ParentId = c.Int(),
                    Type = c.Int(nullable: false),
                    CommentList_Id = c.Int(nullable: false),
                    VoteList_Id = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Folders", t => t.ParentId)
                .ForeignKey("dbo.CommentLists", t => t.CommentList_Id, cascadeDelete: true)
                .ForeignKey("dbo.VoteLists", t => t.VoteList_Id, cascadeDelete: true)
                .Index(t => t.ParentId)
                .Index(t => t.CommentList_Id)
                .Index(t => t.VoteList_Id);

            CreateTable(
                "dbo.Files",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Title = c.String(),
                    Type = c.String(),
                    CommentList_Id = c.Int(nullable: false),
                    Parent_Id = c.Int(),
                    ParentFile_Id = c.Int(),
                    User_Id = c.String(maxLength: 128),
                    VoteList_Id = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CommentLists", t => t.CommentList_Id, cascadeDelete: true)
                .ForeignKey("dbo.Folders", t => t.Parent_Id)
                .ForeignKey("dbo.Files", t => t.ParentFile_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .ForeignKey("dbo.VoteLists", t => t.VoteList_Id, cascadeDelete: true)
                .Index(t => t.CommentList_Id)
                .Index(t => t.Parent_Id)
                .Index(t => t.ParentFile_Id)
                .Index(t => t.User_Id)
                .Index(t => t.VoteList_Id);

            CreateTable(
                "dbo.VoteLists",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.Votes",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Up = c.Boolean(nullable: false),
                    User_Id = c.String(maxLength: 128),
                    VoteList_Id = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .ForeignKey("dbo.VoteLists", t => t.VoteList_Id)
                .Index(t => t.User_Id)
                .Index(t => t.VoteList_Id);

            CreateTable(
                "dbo.Panoptoes",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    PanoptoId = c.String(),
                    Title = c.String(),
                    CommentList_Id = c.Int(nullable: false),
                    Parent_Id = c.Int(),
                    Uploader_Id = c.String(maxLength: 128),
                    VoteList_Id = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CommentLists", t => t.CommentList_Id, cascadeDelete: true)
                .ForeignKey("dbo.Folders", t => t.Parent_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Uploader_Id)
                .ForeignKey("dbo.VoteLists", t => t.VoteList_Id, cascadeDelete: true)
                .Index(t => t.CommentList_Id)
                .Index(t => t.Parent_Id)
                .Index(t => t.Uploader_Id)
                .Index(t => t.VoteList_Id);

            CreateTable(
                "dbo.AspNetRoles",
                c => new
                {
                    Id = c.String(nullable: false, maxLength: 128),
                    Name = c.String(nullable: false, maxLength: 256),
                })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");

            CreateTable(
                "dbo.FolderApplicationUsers",
                c => new
                {
                    Folder_Id = c.Int(nullable: false),
                    ApplicationUser_Id = c.String(nullable: false, maxLength: 128),
                })
                .PrimaryKey(t => new { t.Folder_Id, t.ApplicationUser_Id })
                .ForeignKey("dbo.Folders", t => t.Folder_Id, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id, cascadeDelete: true)
                .Index(t => t.Folder_Id)
                .Index(t => t.ApplicationUser_Id);
        }

        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Comments", "CommentList_Id", "dbo.CommentLists");
            DropForeignKey("dbo.Comments", "VoteList_Id", "dbo.VoteLists");
            DropForeignKey("dbo.Comments", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Folders", "VoteList_Id", "dbo.VoteLists");
            DropForeignKey("dbo.FolderApplicationUsers", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.FolderApplicationUsers", "Folder_Id", "dbo.Folders");
            DropForeignKey("dbo.Panoptoes", "VoteList_Id", "dbo.VoteLists");
            DropForeignKey("dbo.Panoptoes", "Uploader_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Panoptoes", "Parent_Id", "dbo.Folders");
            DropForeignKey("dbo.Panoptoes", "CommentList_Id", "dbo.CommentLists");
            DropForeignKey("dbo.Files", "VoteList_Id", "dbo.VoteLists");
            DropForeignKey("dbo.Votes", "VoteList_Id", "dbo.VoteLists");
            DropForeignKey("dbo.Votes", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Files", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Files", "ParentFile_Id", "dbo.Files");
            DropForeignKey("dbo.Files", "Parent_Id", "dbo.Folders");
            DropForeignKey("dbo.Files", "CommentList_Id", "dbo.CommentLists");
            DropForeignKey("dbo.Folders", "CommentList_Id", "dbo.CommentLists");
            DropForeignKey("dbo.Folders", "ParentId", "dbo.Folders");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Notifications", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.FolderApplicationUsers", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.FolderApplicationUsers", new[] { "Folder_Id" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Panoptoes", new[] { "VoteList_Id" });
            DropIndex("dbo.Panoptoes", new[] { "Uploader_Id" });
            DropIndex("dbo.Panoptoes", new[] { "Parent_Id" });
            DropIndex("dbo.Panoptoes", new[] { "CommentList_Id" });
            DropIndex("dbo.Votes", new[] { "VoteList_Id" });
            DropIndex("dbo.Votes", new[] { "User_Id" });
            DropIndex("dbo.Files", new[] { "VoteList_Id" });
            DropIndex("dbo.Files", new[] { "User_Id" });
            DropIndex("dbo.Files", new[] { "ParentFile_Id" });
            DropIndex("dbo.Files", new[] { "Parent_Id" });
            DropIndex("dbo.Files", new[] { "CommentList_Id" });
            DropIndex("dbo.Folders", new[] { "VoteList_Id" });
            DropIndex("dbo.Folders", new[] { "CommentList_Id" });
            DropIndex("dbo.Folders", new[] { "ParentId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.Notifications", new[] { "User_Id" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Comments", new[] { "CommentList_Id" });
            DropIndex("dbo.Comments", new[] { "VoteList_Id" });
            DropIndex("dbo.Comments", new[] { "User_Id" });
            DropTable("dbo.FolderApplicationUsers");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Panoptoes");
            DropTable("dbo.Votes");
            DropTable("dbo.VoteLists");
            DropTable("dbo.Files");
            DropTable("dbo.Folders");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.Notifications");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Comments");
            DropTable("dbo.CommentLists");
        }
    }
}