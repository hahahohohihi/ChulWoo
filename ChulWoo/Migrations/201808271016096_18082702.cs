namespace ChulWoo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _18082702 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Payment", "StatementType", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Payment", "StatementType");
        }
    }
}
