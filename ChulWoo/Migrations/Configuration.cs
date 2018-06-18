namespace ChulWoo.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using ChulWoo.Models;
    using System.Collections.Generic;

    internal sealed class Configuration : DbMigrationsConfiguration<ChulWoo.DAL.ChulWooContext> 
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(ChulWoo.DAL.ChulWooContext context)
        {
            var employees = new List<Employee>
            {
                new Employee{DepartmentVn="",DepartmentKr="",Name="Lee Kang Su",EmployeeNo="CW002NN",BankAccount="0731-000-720-705",BankLocation="VCB Bắc Giang",
                    TexNo ="",JobPosition="Trưởng phòng",Sex="Nam",BirthDate=DateTime.Parse("1978-02-26"),RegistrationNo="M41710840",RegistrationDate=DateTime.Parse("2016-02-12"),
                    RegistrationPosition="Bộ lao động và thương mại Hàn Quốc",Tel1="097-633-0894",Tel2="",Email="",ResignDate=DateTime.Parse("2000-01-01"),ResignNoteVn="",ResignNoteKr="",
                    Adress="Yên Dũng- Bắc Giang",People="",Religion="",Country="Hàn Quốc",EducationLevel="",MajorVn="",MajorKr="",
                    Marriage="Độc thân",DependentChild="",DependentParents=""},
                new Employee{DepartmentVn="",DepartmentKr="",Name="Son Wang Hyun",EmployeeNo="CW003NN",BankAccount="0731-000-741-357",BankLocation="VCB Bắc Giang",
                    TexNo ="",JobPosition="Phó giám đốc",Sex="Nam",BirthDate=DateTime.Parse("1952-03-03"),RegistrationNo="M01255413",RegistrationDate=DateTime.Parse("2010-09-15"),
                    RegistrationPosition="Bộ lao động và thương mại Hàn Quốc",Tel1="098-938-9040",Tel2="",Email="",ResignDate=DateTime.Parse("2000-01-01"),ResignNoteVn="",ResignNoteKr="",
                    Adress="Yên Dũng- Bắc Giang",People="",Religion="",Country="Hàn Quốc",EducationLevel="",MajorVn="",MajorKr="",
                    Marriage="Đã kết hôn",DependentChild="",DependentParents=""},
                new Employee{DepartmentVn="Bộ phận hành chính",DepartmentKr="",Name="Trương Thị Sinh",EmployeeNo="CW0003",BankAccount="0731-000-687-102",BankLocation="VCB Bắc Giang",
                    TexNo ="8119415119",JobPosition="Nấu ăn",Sex="Nữ",BirthDate=DateTime.Parse("1985-07-20"),RegistrationNo="122137505",RegistrationDate=DateTime.Parse("2011-05-20"),
                    RegistrationPosition="CA Bắc Giang",Tel1="01693-242-713",Tel2="",Email="",ResignDate=DateTime.Parse("2000-01-01"),ResignNoteVn="",ResignNoteKr="",
                    Adress="An Thịnh - Tiền Phong - Yên Dũng- Bắc Giang",People="Kinh",Religion="",Country="Việt Nam",EducationLevel="",MajorVn="",MajorKr="",
                    Marriage="Đã kết hôn",DependentChild="",DependentParents=""},
                new Employee{DepartmentVn="Bộ phận hành chính",DepartmentKr="",Name="Nguyễn Thị Phích",EmployeeNo="CW0007",BankAccount="0731-000-687-103",BankLocation="VCB Bắc Giang",
                    TexNo ="8317114307",JobPosition="Nhân viên phụ bếp",Sex="Nữ",BirthDate=DateTime.Parse("1968-07-10"),RegistrationNo="121070099",RegistrationDate=DateTime.Parse("2012-10-06"),
                    RegistrationPosition="CA Bắc Giang",Tel1="01632-753-953",Tel2="",Email="",ResignDate=DateTime.Parse("2000-01-01"),ResignNoteVn="",ResignNoteKr="",
                    Adress="Thôn Lực - Tân Mỹ - Bắc Giang",People="Kinh",Religion="",Country="Việt Nam",EducationLevel="",MajorVn="",MajorKr="",
                    Marriage="Đã kết hôn",DependentChild="",DependentParents=""},
                new Employee{DepartmentVn="Bộ phận hành chính",DepartmentKr="",Name="Lê Thị Cầu",EmployeeNo="CW0038",BankAccount="0731000735851",BankLocation="VCB Bắc Giang",
                    TexNo ="8461765742",JobPosition="Nhân viên vệ sinh",Sex="Nữ",BirthDate=DateTime.Parse("1968-01-01"),RegistrationNo="121998148",RegistrationDate=DateTime.Parse("2008-04-27"),
                    RegistrationPosition="CA Bắc Giang",Tel1="0962-237-350",Tel2="01634-440-115",Email="",ResignDate=DateTime.Parse("2000-01-01"),ResignNoteVn="",ResignNoteKr="",
                    Adress="Việt Thắng - Đồng Phúc - Yên Dũng - Bắc Giang",People="Kinh",Religion="",Country="Việt Nam",EducationLevel="",MajorVn="",MajorKr="",
                    Marriage="Đã kết hôn",DependentChild="",DependentParents=""},
                new Employee{DepartmentVn="Bộ phận hành chính",DepartmentKr="",Name="Lý Thị Hải Yến",EmployeeNo="",BankAccount="0731-000-732-107",BankLocation="VCB Bắc Giang",
                    TexNo ="8251644124",JobPosition="Thời vụ bếp",Sex="Nữ",BirthDate=DateTime.Parse("1994-06-29"),RegistrationNo="122051524",RegistrationDate=DateTime.Parse("2012-02-07"),
                    RegistrationPosition="CA Bắc Giang",Tel1="01693-242-713",Tel2="",Email="",ResignDate=DateTime.Parse("2000-01-01"),ResignNoteVn="",ResignNoteKr="",
                    Adress="Song Khê - Bắc Giang",People="Kinh",Religion="",Country="Việt Nam",EducationLevel="",MajorVn="",MajorKr="",
                    Marriage="Đã kết hôn",DependentChild="",DependentParents=""},
                new Employee{DepartmentVn="Bộ phận hành chính",DepartmentKr="",Name="Ngô Thị Duyên",EmployeeNo="",BankAccount="0731-000-694-378",BankLocation="VCB Bắc Giang",
                    TexNo ="8363614424",JobPosition="NV phiên dịch",Sex="Nữ",BirthDate=DateTime.Parse("1993-06-07"),RegistrationNo="163260397",RegistrationDate=DateTime.Parse("2013-06-11"),
                    RegistrationPosition="CA Nam Định",Tel1="0979-006-493",Tel2="",Email="duyenktx67@gmail.com",ResignDate=DateTime.Parse("2016-08-06"),ResignNoteVn="Đi du học",ResignNoteKr="",
                    Adress="Xuân Phong - Xuân Trường - Nam Định",People="Kinh",Religion="",Country="Việt Nam",EducationLevel="Đại học",MajorVn="",MajorKr="",
                    Marriage="Độc thân",DependentChild="",DependentParents=""}

            };
            employees.ForEach(s => context.Employees.AddOrUpdate(p => p.Name, s));
            context.SaveChanges();

            var contracts = new List<Contract>
            {
                new Contract{EmployeeID= employees.Single(e => e.Name == "Trương Thị Sinh").ID,
                    StartDate=DateTime.Parse("2016-04-26"),EndDate=DateTime.Parse("2016-05-31"),Type="apprentice",Salary=5000000},
                new Contract{EmployeeID= employees.Single(e => e.Name == "Trương Thị Sinh").ID,
                    StartDate=DateTime.Parse("2016-06-01"),EndDate=DateTime.Parse("2017-06-30"),Type="employee",Salary=6000000},
                new Contract{EmployeeID= employees.Single(e => e.Name == "Nguyễn Thị Phích").ID,
                    StartDate=DateTime.Parse("2016-04-25"),EndDate=DateTime.Parse("2017-04-30"),Type="employee",Salary=3800000}
            };
            contracts.ForEach(s => context.Contracts.Add(s));
            context.SaveChanges();

            var personnels = new List<Personnel>
            {
                new Personnel{EmployeeID= employees.Single(e => e.Name == "Nguyễn Thị Phích").ID,
                    StartDate=DateTime.Parse("2016-05-25"),EndDate=DateTime.Parse("2016-05-25"),Type="annual holiday"},
                new Personnel{EmployeeID= employees.Single(e => e.Name == "Nguyễn Thị Phích").ID,
                    StartDate=DateTime.Parse("2016-06-25"),EndDate=DateTime.Parse("2016-06-25"),Type="unpaid leave"},
                new Personnel{EmployeeID= employees.Single(e => e.Name == "Nguyễn Thị Phích").ID,
                    StartDate=DateTime.Parse("2016-07-25"),EndDate=DateTime.Parse("2016-07-25"),Type="early leave"},
                new Personnel{EmployeeID= employees.Single(e => e.Name == "Nguyễn Thị Phích").ID,
                    StartDate=DateTime.Parse("2016-08-25"),EndDate=DateTime.Parse("2016-08-25"),Type="sick leave"},
                new Personnel{EmployeeID= employees.Single(e => e.Name == "Nguyễn Thị Phích").ID,
                    StartDate=DateTime.Parse("2016-09-25"),EndDate=DateTime.Parse("2016-09-25"),Type="annual holiday"}
            };
            personnels.ForEach(s => context.Personnels.Add(s));
            context.SaveChanges();
        }
    }
}
