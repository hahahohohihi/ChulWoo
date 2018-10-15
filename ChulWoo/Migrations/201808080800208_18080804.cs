namespace ChulWoo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _18080804 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MaterialBuy", "EmployeeID", c => c.Int());
            CreateIndex("dbo.MaterialBuy", "EmployeeID");
            AddForeignKey("dbo.MaterialBuy", "EmployeeID", "dbo.Employee", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MaterialBuy", "EmployeeID", "dbo.Employee");
            DropIndex("dbo.MaterialBuy", new[] { "EmployeeID" });
            DropColumn("dbo.MaterialBuy", "EmployeeID");
        }
    }
}
