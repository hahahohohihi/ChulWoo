namespace ChulWoo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _180824 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Personnel", "HalfDay", c => c.Boolean(nullable: false));
            AddColumn("dbo.Company", "BankAccount", c => c.String(maxLength: 50));
            AddColumn("dbo.Company", "BankLocation", c => c.String());
            AddColumn("dbo.User", "LastLogin", c => c.DateTime());
            AlterColumn("dbo.Company", "Name", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Company", "Name", c => c.String(maxLength: 50));
            DropColumn("dbo.User", "LastLogin");
            DropColumn("dbo.Company", "BankLocation");
            DropColumn("dbo.Company", "BankAccount");
            DropColumn("dbo.Personnel", "HalfDay");
        }
    }
}
