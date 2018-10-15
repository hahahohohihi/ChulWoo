namespace ChulWoo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _180815 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UploadFile", "Date", c => c.DateTime());
            AddColumn("dbo.MaterialName", "NameVn", c => c.String(maxLength: 50));
            AddColumn("dbo.MaterialName", "NameKr", c => c.String(maxLength: 50));

//            AlterColumn("dbo.Employee", "Sex", c => c.Int(nullable: true));
            AddColumn("dbo.Employee", "SexTmp", c => c.Int(nullable: true));
//            Sql("Update dbo.TableName SET ColumnNameTmp = Convert(varbinary, ColumnName)");
            DropColumn("dbo.Employee", "Sex");
            RenameColumn("dbo.Employee", "SexTmp", "Sex");

//            AlterColumn("dbo.Employee", "Marriage", c => c.Int(nullable: true));
            AddColumn("dbo.Employee", "MarriageTmp", c => c.Int(nullable: true));
            DropColumn("dbo.Employee", "Marriage");
            RenameColumn("dbo.Employee", "MarriageTmp", "Marriage");

//            AlterColumn("dbo.Contract", "Type", c => c.Int(nullable: true));
            AddColumn("dbo.Contract", "TypeTmp", c => c.Int(nullable: true));
            DropColumn("dbo.Contract", "Type");
            RenameColumn("dbo.Contract", "TypeTmp", "Type");

            DropColumn("dbo.MaterialName", "Name");
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
                        Sex = p.Int(),
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
                        Marriage = p.Int(),
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
                        Sex = p.Int(),
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
                        Marriage = p.Int(),
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
            AddColumn("dbo.MaterialName", "Name", c => c.String(maxLength: 50));

//            AlterColumn("dbo.Contract", "Type", c => c.String(maxLength: 50));
            AddColumn("dbo.Contract", "TypeTmp", c => c.String(maxLength: 50));
            DropColumn("dbo.Contract", "Type");
            RenameColumn("dbo.Contract", "TypeTmp", "Type");

//            AlterColumn("dbo.Employee", "Marriage", c => c.String(maxLength: 50));
            AddColumn("dbo.Employee", "MarriageTmp", c => c.String(maxLength: 50));
            DropColumn("dbo.Employee", "Marriage");
            RenameColumn("dbo.Employee", "MarriageTmp", "Marriage");

//            AlterColumn("dbo.Employee", "Sex", c => c.String(maxLength: 50));
            AddColumn("dbo.Employee", "SexTmp", c => c.String(maxLength: 50));
            DropColumn("dbo.Employee", "Sex");
            RenameColumn("dbo.Employee", "SexTmp", "Sex");

            DropColumn("dbo.MaterialName", "NameKr");
            DropColumn("dbo.MaterialName", "NameVn");
            DropColumn("dbo.UploadFile", "Date");
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
