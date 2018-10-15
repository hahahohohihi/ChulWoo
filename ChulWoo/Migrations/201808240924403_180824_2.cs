namespace ChulWoo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _180824_2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Payment", "CompanyID", c => c.Int());
            CreateIndex("dbo.Payment", "CompanyID");
            AddForeignKey("dbo.Payment", "CompanyID", "dbo.Company", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Payment", "CompanyID", "dbo.Company");
            DropIndex("dbo.Payment", new[] { "CompanyID" });
            DropColumn("dbo.Payment", "CompanyID");
        }
    }
}
