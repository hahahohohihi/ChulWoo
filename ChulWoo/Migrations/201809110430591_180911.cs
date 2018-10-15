namespace ChulWoo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _180911 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FactoryWorkUnit", "ProjectID", c => c.Int());
            CreateIndex("dbo.FactoryWorkUnit", "ProjectID");
            AddForeignKey("dbo.FactoryWorkUnit", "ProjectID", "dbo.Project", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.FactoryWorkUnit", "ProjectID", "dbo.Project");
            DropIndex("dbo.FactoryWorkUnit", new[] { "ProjectID" });
            DropColumn("dbo.FactoryWorkUnit", "ProjectID");
        }
    }
}
