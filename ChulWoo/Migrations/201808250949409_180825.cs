namespace ChulWoo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _180825 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Personnel", "Type", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Personnel", "Type", c => c.Int());
        }
    }
}
