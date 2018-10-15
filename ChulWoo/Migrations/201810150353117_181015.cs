namespace ChulWoo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _181015 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MaterialUnitPrice", "Currency", c => c.Int(nullable: false));
            AddColumn("dbo.Payment", "Currency", c => c.Int(nullable: false));
            DropColumn("dbo.MaterialUnitPrice", "Unit");
        }
        
        public override void Down()
        {
            AddColumn("dbo.MaterialUnitPrice", "Unit", c => c.Int(nullable: false));
            DropColumn("dbo.Payment", "Currency");
            DropColumn("dbo.MaterialUnitPrice", "Currency");
        }
    }
}
