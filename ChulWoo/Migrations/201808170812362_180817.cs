namespace ChulWoo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _180817 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Payment",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        EmployeeID = c.Int(nullable: false),
                        MaterialBuyID = c.Int(),
                        Date = c.DateTime(),
                        Amount = c.Single(nullable: false),
                        Type = c.Int(),
                        NoteVn = c.String(),
                        NoteKr = c.String(),
                        Translate = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Employee", t => t.EmployeeID, cascadeDelete: true)
                .ForeignKey("dbo.MaterialBuy", t => t.MaterialBuyID)
                .Index(t => t.EmployeeID)
                .Index(t => t.MaterialBuyID);
            
            AddColumn("dbo.MaterialName", "Translate", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Payment", "MaterialBuyID", "dbo.MaterialBuy");
            DropForeignKey("dbo.Payment", "EmployeeID", "dbo.Employee");
            DropIndex("dbo.Payment", new[] { "MaterialBuyID" });
            DropIndex("dbo.Payment", new[] { "EmployeeID" });
            DropColumn("dbo.MaterialName", "Translate");
            DropTable("dbo.Payment");
        }
    }
}
