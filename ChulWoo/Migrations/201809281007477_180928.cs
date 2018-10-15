namespace ChulWoo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _180928 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.MaterialBuy", "VATPer", c => c.Double(nullable: false));
            AlterColumn("dbo.MaterialBuyUnit", "Quantity", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.MaterialBuyUnit", "Quantity", c => c.Single());
            AlterColumn("dbo.MaterialBuy", "VATPer", c => c.Single(nullable: false));
        }
    }
}
