namespace ChulWoo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class InitialCreate : DbMigration 
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Contract",
                c => new
                {
                    ID = c.Int(nullable: false, identity: true),
                    EmployeeID = c.Int(nullable: false),
                    StartDate = c.DateTime(),
                    EndDate = c.DateTime(),
                    Type = c.String(maxLength: 50),
                    Salary = c.Int(),
                })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Employee", t => t.EmployeeID, cascadeDelete: true)
                .Index(t => t.EmployeeID);

            CreateTable(
                "dbo.Employee",
                c => new
                {
                    ID = c.Int(nullable: false, identity: true),
                    DepartmentVn = c.String(maxLength: 50),
                    DepartmentKr = c.String(maxLength: 50),
                    Name = c.String(nullable: false, maxLength: 50),
                    EmployeeNo = c.String(maxLength: 50),
                    BankAccount = c.String(maxLength: 50),
                    BankLocation = c.String(),
                    TexNo = c.String(maxLength: 50),
                    JobPosition = c.String(maxLength: 50),
                    Sex = c.String(maxLength: 50),
                    BirthDate = c.DateTime(),
                    RegistrationNo = c.String(maxLength: 50),
                    RegistrationDate = c.DateTime(),
                    RegistrationPosition = c.String(),
                    Tel1 = c.String(maxLength: 50),
                    Tel2 = c.String(maxLength: 50),
                    Email = c.String(maxLength: 50),
                    ResignDate = c.DateTime(),
                    ResignNoteVn = c.String(),
                    ResignNoteKr = c.String(),
                    Adress = c.String(),
                    People = c.String(maxLength: 50),
                    Religion = c.String(maxLength: 50),
                    Country = c.String(maxLength: 50),
                    EducationLevel = c.String(maxLength: 50),
                    MajorVn = c.String(maxLength: 50),
                    MajorKr = c.String(maxLength: 50),
                    Marriage = c.String(maxLength: 50),
                    DependentChild = c.String(maxLength: 50),
                    DependentParents = c.String(maxLength: 50),
                })
                .PrimaryKey(t => t.ID);

            CreateTable(
                "dbo.Personnel",
                c => new
                {
                    ID = c.Int(nullable: false, identity: true),
                    EmployeeID = c.Int(nullable: false),
                    StartDate = c.DateTime(),
                    EndDate = c.DateTime(),
                    Type = c.String(maxLength: 50),
                })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Employee", t => t.EmployeeID, cascadeDelete: true)
                .Index(t => t.EmployeeID);

        }

        public override void Down()
        {
            DropForeignKey("dbo.Personnel", "EmployeeID", "dbo.Employee");
            DropForeignKey("dbo.Contract", "EmployeeID", "dbo.Employee");
            DropIndex("dbo.Personnel", new[] { "EmployeeID" });
            DropIndex("dbo.Contract", new[] { "EmployeeID" });
            DropTable("dbo.Personnel");
            DropTable("dbo.Employee");
            DropTable("dbo.Contract");
        }
    }
}
