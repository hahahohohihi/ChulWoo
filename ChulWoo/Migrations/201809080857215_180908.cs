namespace ChulWoo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _180908 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FactoryDailyWork",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ProparingPersonID = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        NoteVn = c.String(),
                        NoteKr = c.String(),
                        Translate = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Employee", t => t.ProparingPersonID, cascadeDelete: true)
                .Index(t => t.ProparingPersonID);
            
            CreateTable(
                "dbo.FactoryWorkUnit",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        FactoryDailyWorkID = c.Int(nullable: false),
                        EquipCount = c.Double(nullable: false),
                        NoteVn = c.String(),
                        NoteKr = c.String(),
                        Translate = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.FactoryDailyWork", t => t.FactoryDailyWorkID, cascadeDelete: true)
                .Index(t => t.FactoryDailyWorkID);
            
            AddColumn("dbo.MaterialBuy", "VATPer", c => c.Single(nullable: false));
            AddColumn("dbo.User", "Language", c => c.Int(nullable: false));
            AlterColumn("dbo.MaterialBuy", "NoteVn", c => c.String(maxLength: 100));
            AlterColumn("dbo.MaterialBuy", "NoteKr", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.FactoryDailyWork", "ProparingPersonID", "dbo.Employee");
            DropForeignKey("dbo.FactoryWorkUnit", "FactoryDailyWorkID", "dbo.FactoryDailyWork");
            DropIndex("dbo.FactoryWorkUnit", new[] { "FactoryDailyWorkID" });
            DropIndex("dbo.FactoryDailyWork", new[] { "ProparingPersonID" });
            AlterColumn("dbo.MaterialBuy", "NoteKr", c => c.String());
            AlterColumn("dbo.MaterialBuy", "NoteVn", c => c.String());
            DropColumn("dbo.User", "Language");
            DropColumn("dbo.MaterialBuy", "VATPer");
            DropTable("dbo.FactoryWorkUnit");
            DropTable("dbo.FactoryDailyWork");
        }
    }
}
