namespace ChulWoo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _180827 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Payment", "Project_ID", c => c.Int());
            CreateIndex("dbo.Payment", "Project_ID");
            AddForeignKey("dbo.Payment", "Project_ID", "dbo.Project", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Payment", "Project_ID", "dbo.Project");
            DropIndex("dbo.Payment", new[] { "Project_ID" });
            DropColumn("dbo.Payment", "Project_ID");
        }
    }
}
