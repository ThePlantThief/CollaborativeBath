namespace CollaborativeBath.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class SubbedListsMany2Many : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.CommentLists", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.VoteLists", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.CommentLists", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.VoteLists", new[] { "ApplicationUser_Id" });
            CreateTable(
                "dbo.VoteListApplicationUsers",
                c => new
                {
                    VoteList_Id = c.Int(nullable: false),
                    ApplicationUser_Id = c.String(nullable: false, maxLength: 128),
                })
                .PrimaryKey(t => new { t.VoteList_Id, t.ApplicationUser_Id })
                .ForeignKey("dbo.VoteLists", t => t.VoteList_Id, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id, cascadeDelete: true)
                .Index(t => t.VoteList_Id)
                .Index(t => t.ApplicationUser_Id);

            CreateTable(
                "dbo.CommentListApplicationUsers",
                c => new
                {
                    CommentList_Id = c.Int(nullable: false),
                    ApplicationUser_Id = c.String(nullable: false, maxLength: 128),
                })
                .PrimaryKey(t => new { t.CommentList_Id, t.ApplicationUser_Id })
                .ForeignKey("dbo.CommentLists", t => t.CommentList_Id, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id, cascadeDelete: true)
                .Index(t => t.CommentList_Id)
                .Index(t => t.ApplicationUser_Id);

            DropColumn("dbo.CommentLists", "ApplicationUser_Id");
            DropColumn("dbo.VoteLists", "ApplicationUser_Id");
        }

        public override void Down()
        {
            AddColumn("dbo.VoteLists", "ApplicationUser_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.CommentLists", "ApplicationUser_Id", c => c.String(maxLength: 128));
            DropForeignKey("dbo.CommentListApplicationUsers", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.CommentListApplicationUsers", "CommentList_Id", "dbo.CommentLists");
            DropForeignKey("dbo.VoteListApplicationUsers", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.VoteListApplicationUsers", "VoteList_Id", "dbo.VoteLists");
            DropIndex("dbo.CommentListApplicationUsers", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.CommentListApplicationUsers", new[] { "CommentList_Id" });
            DropIndex("dbo.VoteListApplicationUsers", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.VoteListApplicationUsers", new[] { "VoteList_Id" });
            DropTable("dbo.CommentListApplicationUsers");
            DropTable("dbo.VoteListApplicationUsers");
            CreateIndex("dbo.VoteLists", "ApplicationUser_Id");
            CreateIndex("dbo.CommentLists", "ApplicationUser_Id");
            AddForeignKey("dbo.VoteLists", "ApplicationUser_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.CommentLists", "ApplicationUser_Id", "dbo.AspNetUsers", "Id");
        }
    }
}