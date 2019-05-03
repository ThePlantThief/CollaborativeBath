namespace CollaborativeBath.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class SubbedLists : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CommentLists", "ApplicationUser_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.VoteLists", "ApplicationUser_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.CommentLists", "ApplicationUser_Id");
            CreateIndex("dbo.VoteLists", "ApplicationUser_Id");
            AddForeignKey("dbo.CommentLists", "ApplicationUser_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.VoteLists", "ApplicationUser_Id", "dbo.AspNetUsers", "Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.VoteLists", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.CommentLists", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.VoteLists", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.CommentLists", new[] { "ApplicationUser_Id" });
            DropColumn("dbo.VoteLists", "ApplicationUser_Id");
            DropColumn("dbo.CommentLists", "ApplicationUser_Id");
        }
    }
}