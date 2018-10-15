namespace ChulWoo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _18092102 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.PaymentMaterialBuy", newName: "MaterialBuyPayment");
            RenameColumn(table: "dbo.MaterialBuyPayment", name: "Payment_ID", newName: "PaymentID");
            RenameColumn(table: "dbo.MaterialBuyPayment", name: "MaterialBuy_ID", newName: "MaterialBuyID");
            RenameIndex(table: "dbo.MaterialBuyPayment", name: "IX_MaterialBuy_ID", newName: "IX_MaterialBuyID");
            RenameIndex(table: "dbo.MaterialBuyPayment", name: "IX_Payment_ID", newName: "IX_PaymentID");
            DropPrimaryKey("dbo.MaterialBuyPayment");
            AddPrimaryKey("dbo.MaterialBuyPayment", new[] { "MaterialBuyID", "PaymentID" });
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.MaterialBuyPayment");
            AddPrimaryKey("dbo.MaterialBuyPayment", new[] { "Payment_ID", "MaterialBuy_ID" });
            RenameIndex(table: "dbo.MaterialBuyPayment", name: "IX_PaymentID", newName: "IX_Payment_ID");
            RenameIndex(table: "dbo.MaterialBuyPayment", name: "IX_MaterialBuyID", newName: "IX_MaterialBuy_ID");
            RenameColumn(table: "dbo.MaterialBuyPayment", name: "MaterialBuyID", newName: "MaterialBuy_ID");
            RenameColumn(table: "dbo.MaterialBuyPayment", name: "PaymentID", newName: "Payment_ID");
            RenameTable(name: "dbo.MaterialBuyPayment", newName: "PaymentMaterialBuy");
        }
    }
}
