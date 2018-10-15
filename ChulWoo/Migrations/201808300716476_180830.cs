namespace ChulWoo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _180830 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Project", "CompanyID", "dbo.Company");
            CreateTable(
                "dbo.DailyWork",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ProjectID = c.Int(nullable: false),
                        ProparingPersonID = c.Int(nullable: false),
                        Date = c.DateTime(),
                        PrrocessPerPlan = c.Single(nullable: false),
                        PrrocessPerResult = c.Single(nullable: false),
                        WeatherVn = c.String(),
                        WeatherKr = c.String(),
                        NoteVn = c.String(),
                        NoteKr = c.String(),
                        Translate = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Project", t => t.ProjectID, cascadeDelete: true)
                .ForeignKey("dbo.Employee", t => t.ProparingPersonID, cascadeDelete: true)
                .Index(t => t.ProjectID)
                .Index(t => t.ProparingPersonID);
            
            CreateTable(
                "dbo.EquipmentUnit",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        DailyWorkID = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        NameVn = c.String(),
                        NameKr = c.String(),
                        NoteVn = c.String(),
                        NoteKr = c.String(),
                        EquipCount = c.Double(nullable: false),
                        Translate = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.DailyWork", t => t.DailyWorkID, cascadeDelete: true)
                .Index(t => t.DailyWorkID);
            
            CreateTable(
                "dbo.WorkUnit",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        DailyWorkID = c.Int(nullable: false),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(),
                        Complete = c.Boolean(nullable: false),
                        WorkNameVn = c.String(),
                        WorkNameKr = c.String(),
                        NoteVn = c.String(),
                        NoteKr = c.String(),
                        Translate = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.DailyWork", t => t.DailyWorkID, cascadeDelete: true)
                .Index(t => t.DailyWorkID);
            
            AddColumn("dbo.UploadFile", "DailyWork_ID", c => c.Int());
            AddColumn("dbo.Project", "ConstructorID", c => c.Int());
            AddColumn("dbo.Project", "ManagerID", c => c.Int());
            AddColumn("dbo.Project", "Company_ID", c => c.Int());
            CreateIndex("dbo.UploadFile", "DailyWork_ID");
            CreateIndex("dbo.Project", "ConstructorID");
            CreateIndex("dbo.Project", "ManagerID");
            CreateIndex("dbo.Project", "Company_ID");
            AddForeignKey("dbo.Project", "ConstructorID", "dbo.Company", "ID");
            AddForeignKey("dbo.UploadFile", "DailyWork_ID", "dbo.DailyWork", "ID");
            AddForeignKey("dbo.Project", "ManagerID", "dbo.Employee", "ID");
            AddForeignKey("dbo.Project", "Company_ID", "dbo.Company", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Project", "Company_ID", "dbo.Company");
            DropForeignKey("dbo.Project", "ManagerID", "dbo.Employee");
            DropForeignKey("dbo.WorkUnit", "DailyWorkID", "dbo.DailyWork");
            DropForeignKey("dbo.UploadFile", "DailyWork_ID", "dbo.DailyWork");
            DropForeignKey("dbo.DailyWork", "ProparingPersonID", "dbo.Employee");
            DropForeignKey("dbo.DailyWork", "ProjectID", "dbo.Project");
            DropForeignKey("dbo.EquipmentUnit", "DailyWorkID", "dbo.DailyWork");
            DropForeignKey("dbo.Project", "ConstructorID", "dbo.Company");
            DropIndex("dbo.WorkUnit", new[] { "DailyWorkID" });
            DropIndex("dbo.EquipmentUnit", new[] { "DailyWorkID" });
            DropIndex("dbo.DailyWork", new[] { "ProparingPersonID" });
            DropIndex("dbo.DailyWork", new[] { "ProjectID" });
            DropIndex("dbo.Project", new[] { "Company_ID" });
            DropIndex("dbo.Project", new[] { "ManagerID" });
            DropIndex("dbo.Project", new[] { "ConstructorID" });
            DropIndex("dbo.UploadFile", new[] { "DailyWork_ID" });
            DropColumn("dbo.Project", "Company_ID");
            DropColumn("dbo.Project", "ManagerID");
            DropColumn("dbo.Project", "ConstructorID");
            DropColumn("dbo.UploadFile", "DailyWork_ID");
            DropTable("dbo.WorkUnit");
            DropTable("dbo.EquipmentUnit");
            DropTable("dbo.DailyWork");
            AddForeignKey("dbo.Project", "CompanyID", "dbo.Company", "ID", cascadeDelete: true);
        }
    }
}
