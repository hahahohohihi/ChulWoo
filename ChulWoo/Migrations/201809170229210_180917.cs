namespace ChulWoo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _180917 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UploadFile", "Security", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.UploadFile", "Security");
        }
    }
}
