namespace ChulWoo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeMaterialBuy : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MaterialBuy", "Note", c => c.String());
            AddColumn("dbo.MaterialBuy", "VAT", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.MaterialBuy", "VAT");
            DropColumn("dbo.MaterialBuy", "Note");
        }
    }
}
