namespace ChulWoo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _180820 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MaterialUnitPrice", "UnitString", c => c.String(maxLength: 20));
            AlterColumn("dbo.MaterialUnitPrice", "Price", c => c.Double(nullable: false));
            AlterColumn("dbo.Payment", "Amount", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Payment", "Amount", c => c.Single(nullable: false));
            AlterColumn("dbo.MaterialUnitPrice", "Price", c => c.Single(nullable: false));
            DropColumn("dbo.MaterialUnitPrice", "UnitString");
        }
    }
}
