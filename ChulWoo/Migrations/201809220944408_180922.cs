namespace ChulWoo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _180922 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MaterialBuy", "MaterialBuyType", c => c.Int(nullable: false));
            DropColumn("dbo.MaterialBuy", "VAT");
        }
        
        public override void Down()
        {
            AddColumn("dbo.MaterialBuy", "VAT", c => c.Boolean(nullable: false));
            DropColumn("dbo.MaterialBuy", "MaterialBuyType");
        }
    }
}
