namespace ChulWoo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _180831 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.WorkUnit", "DailyWorkID", "dbo.DailyWork");
            DropIndex("dbo.WorkUnit", new[] { "DailyWorkID" });
            AddColumn("dbo.DailyWork", "WorkUnit_ID", c => c.Int());
            AddColumn("dbo.WorkUnit", "Project_ID", c => c.Int());
            CreateIndex("dbo.DailyWork", "WorkUnit_ID");
            CreateIndex("dbo.WorkUnit", "Project_ID");
            AddForeignKey("dbo.DailyWork", "WorkUnit_ID", "dbo.WorkUnit", "ID");
            AddForeignKey("dbo.WorkUnit", "Project_ID", "dbo.Project", "ID");
            DropColumn("dbo.WorkUnit", "DailyWorkID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.WorkUnit", "DailyWorkID", c => c.Int(nullable: false));
            DropForeignKey("dbo.WorkUnit", "Project_ID", "dbo.Project");
            DropForeignKey("dbo.DailyWork", "WorkUnit_ID", "dbo.WorkUnit");
            DropIndex("dbo.WorkUnit", new[] { "Project_ID" });
            DropIndex("dbo.DailyWork", new[] { "WorkUnit_ID" });
            DropColumn("dbo.WorkUnit", "Project_ID");
            DropColumn("dbo.DailyWork", "WorkUnit_ID");
            CreateIndex("dbo.WorkUnit", "DailyWorkID");
            AddForeignKey("dbo.WorkUnit", "DailyWorkID", "dbo.DailyWork", "ID", cascadeDelete: true);
        }
    }
}
