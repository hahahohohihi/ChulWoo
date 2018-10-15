namespace ChulWoo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _180828 : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Payment", name: "Project_ID", newName: "ProjectID");
            RenameIndex(table: "dbo.Payment", name: "IX_Project_ID", newName: "IX_ProjectID");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Payment", name: "IX_ProjectID", newName: "IX_Project_ID");
            RenameColumn(table: "dbo.Payment", name: "ProjectID", newName: "Project_ID");
        }
    }
}
