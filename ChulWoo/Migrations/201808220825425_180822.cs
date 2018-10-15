namespace ChulWoo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _180822 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Payment", "AmountString", c => c.String(maxLength: 50));
            AlterColumn("dbo.Payment", "Type", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Payment", "Type", c => c.Int());
            DropColumn("dbo.Payment", "AmountString");
        }
    }
}
