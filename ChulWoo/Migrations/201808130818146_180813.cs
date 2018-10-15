namespace ChulWoo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _180813 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.MaterialBuyUnit", "Quantity", c => c.Single());
            AlterColumn("dbo.MaterialUnitPrice", "Price", c => c.Single(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.MaterialUnitPrice", "Price", c => c.Int(nullable: false));
            AlterColumn("dbo.MaterialBuyUnit", "Quantity", c => c.Int());
        }
    }
}
