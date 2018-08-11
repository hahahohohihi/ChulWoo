namespace ChulWoo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _180810 : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.MaterialBuyUnit", name: "MaterialBuy_ID", newName: "MaterialBuyID");
            RenameIndex(table: "dbo.MaterialBuyUnit", name: "IX_MaterialBuy_ID", newName: "IX_MaterialBuyID");
            AddColumn("dbo.Board", "Translate", c => c.Boolean(nullable: false));
            AddColumn("dbo.Employee", "Translate", c => c.Boolean(nullable: false));
            AddColumn("dbo.Personnel", "Name", c => c.String(maxLength: 50));
            AddColumn("dbo.Personnel", "Translate", c => c.Boolean(nullable: false));
            AddColumn("dbo.MaterialBuy", "NoteVn", c => c.String());
            AddColumn("dbo.MaterialBuy", "NoteKr", c => c.String());
            AddColumn("dbo.MaterialBuy", "Translate", c => c.Boolean(nullable: false));
            AddColumn("dbo.Project", "NameVn", c => c.String(maxLength: 50));
            AddColumn("dbo.Project", "NameKr", c => c.String(maxLength: 50));
            AddColumn("dbo.Project", "Translate", c => c.Boolean(nullable: false));
            DropColumn("dbo.MaterialBuy", "Note");
            DropColumn("dbo.Project", "Name");
            AlterStoredProcedure(
                "dbo.Employee_Insert",
                p => new
                    {
                        ResignID = p.Int(),
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
                        ImageData = p.Binary(),
                        ImageMimeType = p.String(),
                        Translate = p.Boolean(),
                    },
                body:
                    @"INSERT [dbo].[Employee]([ResignID], [DepartmentVn], [DepartmentKr], [Name], [EmployeeNo], [BankAccount], [BankLocation], [TexNo], [JobPosition], [Sex], [BirthDate], [RegistrationNo], [RegistrationDate], [RegistrationPosition], [Tel1], [Tel2], [Email], [Adress], [People], [Religion], [Country], [EducationLevel], [MajorVn], [MajorKr], [Marriage], [DependentChild], [DependentParents], [ImageData], [ImageMimeType], [Translate])
                      VALUES (@ResignID, @DepartmentVn, @DepartmentKr, @Name, @EmployeeNo, @BankAccount, @BankLocation, @TexNo, @JobPosition, @Sex, @BirthDate, @RegistrationNo, @RegistrationDate, @RegistrationPosition, @Tel1, @Tel2, @Email, @Adress, @People, @Religion, @Country, @EducationLevel, @MajorVn, @MajorKr, @Marriage, @DependentChild, @DependentParents, @ImageData, @ImageMimeType, @Translate)
                      
                      DECLARE @ID int
                      SELECT @ID = [ID]
                      FROM [dbo].[Employee]
                      WHERE @@ROWCOUNT > 0 AND [ID] = scope_identity()
                      
                      SELECT t0.[ID], t0.[RowVersion]
                      FROM [dbo].[Employee] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[ID] = @ID"
            );
            
            AlterStoredProcedure(
                "dbo.Employee_Update",
                p => new
                    {
                        ID = p.Int(),
                        ResignID = p.Int(),
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
                        ImageData = p.Binary(),
                        ImageMimeType = p.String(),
                        RowVersion_Original = p.Binary(maxLength: 8, fixedLength: true, storeType: "rowversion"),
                        Translate = p.Boolean(),
                    },
                body:
                    @"UPDATE [dbo].[Employee]
                      SET [ResignID] = @ResignID, [DepartmentVn] = @DepartmentVn, [DepartmentKr] = @DepartmentKr, [Name] = @Name, [EmployeeNo] = @EmployeeNo, [BankAccount] = @BankAccount, [BankLocation] = @BankLocation, [TexNo] = @TexNo, [JobPosition] = @JobPosition, [Sex] = @Sex, [BirthDate] = @BirthDate, [RegistrationNo] = @RegistrationNo, [RegistrationDate] = @RegistrationDate, [RegistrationPosition] = @RegistrationPosition, [Tel1] = @Tel1, [Tel2] = @Tel2, [Email] = @Email, [Adress] = @Adress, [People] = @People, [Religion] = @Religion, [Country] = @Country, [EducationLevel] = @EducationLevel, [MajorVn] = @MajorVn, [MajorKr] = @MajorKr, [Marriage] = @Marriage, [DependentChild] = @DependentChild, [DependentParents] = @DependentParents, [ImageData] = @ImageData, [ImageMimeType] = @ImageMimeType, [Translate] = @Translate
                      WHERE (([ID] = @ID) AND (([RowVersion] = @RowVersion_Original) OR ([RowVersion] IS NULL AND @RowVersion_Original IS NULL)))
                      
                      SELECT t0.[RowVersion]
                      FROM [dbo].[Employee] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[ID] = @ID"
            );
            
        }
        
        public override void Down()
        {
            AddColumn("dbo.Project", "Name", c => c.String(maxLength: 50));
            AddColumn("dbo.MaterialBuy", "Note", c => c.String());
            DropColumn("dbo.Project", "Translate");
            DropColumn("dbo.Project", "NameKr");
            DropColumn("dbo.Project", "NameVn");
            DropColumn("dbo.MaterialBuy", "Translate");
            DropColumn("dbo.MaterialBuy", "NoteKr");
            DropColumn("dbo.MaterialBuy", "NoteVn");
            DropColumn("dbo.Personnel", "Translate");
            DropColumn("dbo.Personnel", "Name");
            DropColumn("dbo.Employee", "Translate");
            DropColumn("dbo.Board", "Translate");
            RenameIndex(table: "dbo.MaterialBuyUnit", name: "IX_MaterialBuyID", newName: "IX_MaterialBuy_ID");
            RenameColumn(table: "dbo.MaterialBuyUnit", name: "MaterialBuyID", newName: "MaterialBuy_ID");
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
