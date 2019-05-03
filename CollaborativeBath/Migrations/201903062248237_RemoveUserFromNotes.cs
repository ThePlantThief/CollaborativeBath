namespace CollaborativeBath.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class RemoveUserFromNotes : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Notifications", name: "UserId", newName: "ApplicationUser_Id");
            RenameIndex(table: "dbo.Notifications", name: "IX_UserId", newName: "IX_ApplicationUser_Id");
        }

        public override void Down()
        {
            RenameIndex(table: "dbo.Notifications", name: "IX_ApplicationUser_Id", newName: "IX_UserId");
            RenameColumn(table: "dbo.Notifications", name: "ApplicationUser_Id", newName: "UserId");
        }
    }
}