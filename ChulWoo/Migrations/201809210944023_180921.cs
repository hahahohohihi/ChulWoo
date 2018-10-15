namespace ChulWoo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _180921 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Payment", "MaterialBuyID", "dbo.MaterialBuy");
            DropIndex("dbo.Payment", new[] { "MaterialBuyID" });
            CreateTable(
                "dbo.PaymentMaterialBuy",
                c => new
                    {
                        Payment_ID = c.Int(nullable: false),
                        MaterialBuy_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Payment_ID, t.MaterialBuy_ID })
                .ForeignKey("dbo.Payment", t => t.Payment_ID, cascadeDelete: true)
                .ForeignKey("dbo.MaterialBuy", t => t.MaterialBuy_ID, cascadeDelete: true)
                .Index(t => t.Payment_ID)
                .Index(t => t.MaterialBuy_ID);
            
            DropColumn("dbo.Payment", "MaterialBuyID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Payment", "MaterialBuyID", c => c.Int());
            DropForeignKey("dbo.PaymentMaterialBuy", "MaterialBuy_ID", "dbo.MaterialBuy");
            DropForeignKey("dbo.PaymentMaterialBuy", "Payment_ID", "dbo.Payment");
            DropIndex("dbo.PaymentMaterialBuy", new[] { "MaterialBuy_ID" });
            DropIndex("dbo.PaymentMaterialBuy", new[] { "Payment_ID" });
            DropTable("dbo.PaymentMaterialBuy");
            CreateIndex("dbo.Payment", "MaterialBuyID");
            AddForeignKey("dbo.Payment", "MaterialBuyID", "dbo.MaterialBuy", "ID");
        }
    }
}
