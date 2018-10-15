namespace ChulWoo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _180912 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FactoryWorkUnit", "Type", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.FactoryWorkUnit", "Type");
        }
    }
}
