namespace ChulWoo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _18083102 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.DailyWork", "WorkUnit_ID", "dbo.WorkUnit");
            DropForeignKey("dbo.WorkUnit", "Project_ID", "dbo.Project");
            DropIndex("dbo.DailyWork", new[] { "WorkUnit_ID" });
            DropIndex("dbo.WorkUnit", new[] { "Project_ID" });
            RenameColumn(table: "dbo.WorkUnit", name: "Project_ID", newName: "ProjectID");
            AlterColumn("dbo.WorkUnit", "ProjectID", c => c.Int(nullable: false));
            CreateIndex("dbo.WorkUnit", "ProjectID");
            AddForeignKey("dbo.WorkUnit", "ProjectID", "dbo.Project", "ID", cascadeDelete: true);
            DropColumn("dbo.DailyWork", "WorkUnit_ID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.DailyWork", "WorkUnit_ID", c => c.Int());
            DropForeignKey("dbo.WorkUnit", "ProjectID", "dbo.Project");
            DropIndex("dbo.WorkUnit", new[] { "ProjectID" });
            AlterColumn("dbo.WorkUnit", "ProjectID", c => c.Int());
            RenameColumn(table: "dbo.WorkUnit", name: "ProjectID", newName: "Project_ID");
            CreateIndex("dbo.WorkUnit", "Project_ID");
            CreateIndex("dbo.DailyWork", "WorkUnit_ID");
            AddForeignKey("dbo.WorkUnit", "Project_ID", "dbo.Project", "ID");
            AddForeignKey("dbo.DailyWork", "WorkUnit_ID", "dbo.WorkUnit", "ID");
        }
    }
}
