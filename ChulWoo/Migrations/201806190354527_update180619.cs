namespace ChulWoo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update180619 : DbMigration
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
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
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
            
            CreateTable(
                "dbo.Resign",
                c => new
                    {
                        EmployeeID = c.Int(nullable: false),
                        ResignDate = c.DateTime(),
                        ResignNoteVn = c.String(),
                        ResignNoteKr = c.String(),
                    })
                .PrimaryKey(t => t.EmployeeID)
                .ForeignKey("dbo.Employee", t => t.EmployeeID)
                .Index(t => t.EmployeeID);
            
            CreateStoredProcedure(
                "dbo.Employee_Insert",
                p => new
                    {
                        DepartmentVn = p.String(maxLength: 50),
                        DepartmentKr = p.String(maxLength: 50),
                        Name = p.String(maxLength: 50),
                        EmployeeNo = p.String(maxLength: 50),
                        BankAccount = p.String(maxLength: 50),
                        BankLocation = p.String(),
                        TexNo = p.String(maxLength: 50),
                        JobPosition = p.String(maxLength: 50),
                        Sex = p.String(maxLength: 50),
                        BirthDate = p.DateTime(),
                        RegistrationNo = p.String(maxLength: 50),
                        RegistrationDate = p.DateTime(),
                        RegistrationPosition = p.String(),
                        Tel1 = p.String(maxLength: 50),
                        Tel2 = p.String(maxLength: 50),
                        Email = p.String(maxLength: 50),
                        Adress = p.String(),
                        People = p.String(maxLength: 50),
                        Religion = p.String(maxLength: 50),
                        Country = p.String(maxLength: 50),
                        EducationLevel = p.String(maxLength: 50),
                        MajorVn = p.String(maxLength: 50),
                        MajorKr = p.String(maxLength: 50),
                        Marriage = p.String(maxLength: 50),
                        DependentChild = p.String(maxLength: 50),
                        DependentParents = p.String(maxLength: 50),
                    },
                body:
                    @"INSERT [dbo].[Employee]([DepartmentVn], [DepartmentKr], [Name], [EmployeeNo], [BankAccount], [BankLocation], [TexNo], [JobPosition], [Sex], [BirthDate], [RegistrationNo], [RegistrationDate], [RegistrationPosition], [Tel1], [Tel2], [Email], [Adress], [People], [Religion], [Country], [EducationLevel], [MajorVn], [MajorKr], [Marriage], [DependentChild], [DependentParents])
                      VALUES (@DepartmentVn, @DepartmentKr, @Name, @EmployeeNo, @BankAccount, @BankLocation, @TexNo, @JobPosition, @Sex, @BirthDate, @RegistrationNo, @RegistrationDate, @RegistrationPosition, @Tel1, @Tel2, @Email, @Adress, @People, @Religion, @Country, @EducationLevel, @MajorVn, @MajorKr, @Marriage, @DependentChild, @DependentParents)
                      
                      DECLARE @ID int
                      SELECT @ID = [ID]
                      FROM [dbo].[Employee]
                      WHERE @@ROWCOUNT > 0 AND [ID] = scope_identity()
                      
                      SELECT t0.[ID], t0.[RowVersion]
                      FROM [dbo].[Employee] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[ID] = @ID"
            );
            
            CreateStoredProcedure(
                "dbo.Employee_Update",
                p => new
                    {
                        ID = p.Int(),
                        DepartmentVn = p.String(maxLength: 50),
                        DepartmentKr = p.String(maxLength: 50),
                        Name = p.String(maxLength: 50),
                        EmployeeNo = p.String(maxLength: 50),
                        BankAccount = p.String(maxLength: 50),
                        BankLocation = p.String(),
                        TexNo = p.String(maxLength: 50),
                        JobPosition = p.String(maxLength: 50),
                        Sex = p.String(maxLength: 50),
                        BirthDate = p.DateTime(),
                        RegistrationNo = p.String(maxLength: 50),
                        RegistrationDate = p.DateTime(),
                        RegistrationPosition = p.String(),
                        Tel1 = p.String(maxLength: 50),
                        Tel2 = p.String(maxLength: 50),
                        Email = p.String(maxLength: 50),
                        Adress = p.String(),
                        People = p.String(maxLength: 50),
                        Religion = p.String(maxLength: 50),
                        Country = p.String(maxLength: 50),
                        EducationLevel = p.String(maxLength: 50),
                        MajorVn = p.String(maxLength: 50),
                        MajorKr = p.String(maxLength: 50),
                        Marriage = p.String(maxLength: 50),
                        DependentChild = p.String(maxLength: 50),
                        DependentParents = p.String(maxLength: 50),
                        RowVersion_Original = p.Binary(maxLength: 8, fixedLength: true, storeType: "rowversion"),
                    },
                body:
                    @"UPDATE [dbo].[Employee]
                      SET [DepartmentVn] = @DepartmentVn, [DepartmentKr] = @DepartmentKr, [Name] = @Name, [EmployeeNo] = @EmployeeNo, [BankAccount] = @BankAccount, [BankLocation] = @BankLocation, [TexNo] = @TexNo, [JobPosition] = @JobPosition, [Sex] = @Sex, [BirthDate] = @BirthDate, [RegistrationNo] = @RegistrationNo, [RegistrationDate] = @RegistrationDate, [RegistrationPosition] = @RegistrationPosition, [Tel1] = @Tel1, [Tel2] = @Tel2, [Email] = @Email, [Adress] = @Adress, [People] = @People, [Religion] = @Religion, [Country] = @Country, [EducationLevel] = @EducationLevel, [MajorVn] = @MajorVn, [MajorKr] = @MajorKr, [Marriage] = @Marriage, [DependentChild] = @DependentChild, [DependentParents] = @DependentParents
                      WHERE (([ID] = @ID) AND (([RowVersion] = @RowVersion_Original) OR ([RowVersion] IS NULL AND @RowVersion_Original IS NULL)))
                      
                      SELECT t0.[RowVersion]
                      FROM [dbo].[Employee] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[ID] = @ID"
            );
            
            CreateStoredProcedure(
                "dbo.Employee_Delete",
                p => new
                    {
                        ID = p.Int(),
                        RowVersion_Original = p.Binary(maxLength: 8, fixedLength: true, storeType: "rowversion"),
                    },
                body:
                    @"DELETE [dbo].[Employee]
                      WHERE (([ID] = @ID) AND (([RowVersion] = @RowVersion_Original) OR ([RowVersion] IS NULL AND @RowVersion_Original IS NULL)))"
            );
            
        }
        
        public override void Down()
        {
            DropStoredProcedure("dbo.Employee_Delete");
            DropStoredProcedure("dbo.Employee_Update");
            DropStoredProcedure("dbo.Employee_Insert");
            DropForeignKey("dbo.Resign", "EmployeeID", "dbo.Employee");
            DropForeignKey("dbo.Personnel", "EmployeeID", "dbo.Employee");
            DropForeignKey("dbo.Contract", "EmployeeID", "dbo.Employee");
            DropIndex("dbo.Resign", new[] { "EmployeeID" });
            DropIndex("dbo.Personnel", new[] { "EmployeeID" });
            DropIndex("dbo.Contract", new[] { "EmployeeID" });
            DropTable("dbo.Resign");
            DropTable("dbo.Personnel");
            DropTable("dbo.Employee");
            DropTable("dbo.Contract");
        }
    }
}
