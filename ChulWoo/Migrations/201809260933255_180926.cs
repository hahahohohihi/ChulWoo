namespace ChulWoo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _180926 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.MaterialBuy", "NoteVn", c => c.String());
            AlterColumn("dbo.MaterialBuy", "NoteKr", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.MaterialBuy", "NoteKr", c => c.String(maxLength: 100));
            AlterColumn("dbo.MaterialBuy", "NoteVn", c => c.String(maxLength: 100));
        }
    }
}
