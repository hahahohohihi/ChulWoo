﻿namespace ChulWoo.Migrations
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
            /*
                        var employees = new List<Employee>
                        {
                            new Employee{DepartmentVn="",DepartmentKr="",Name="Lee Kang Su",EmployeeNo="CW002NN",BankAccount="0731-000-720-705",BankLocation="VCB Bắc Giang",
                                TexNo ="",JobPosition="Trưởng phòng",Sex="Nam",BirthDate=DateTime.Parse("1978-02-26"),RegistrationNo="M41710840",RegistrationDate=DateTime.Parse("2016-02-12"),
                                RegistrationPosition="Bộ lao động và thương mại Hàn Quốc",Tel1="097-633-0894",Tel2="",Email="",
                                Adress="Yên Dũng- Bắc Giang",People="",Religion="",Country="Hàn Quốc",EducationLevel="",MajorVn="",MajorKr="",
                                Marriage="Độc thân",DependentChild="",DependentParents=""},
                            new Employee{DepartmentVn="",DepartmentKr="",Name="Son Wang Hyun",EmployeeNo="CW003NN",BankAccount="0731-000-741-357",BankLocation="VCB Bắc Giang",
                                TexNo ="",JobPosition="Phó giám đốc",Sex="Nam",BirthDate=DateTime.Parse("1952-03-03"),RegistrationNo="M01255413",RegistrationDate=DateTime.Parse("2010-09-15"),
                                RegistrationPosition="Bộ lao động và thương mại Hàn Quốc",Tel1="098-938-9040",Tel2="",Email="",
                                Adress="Yên Dũng- Bắc Giang",People="",Religion="",Country="Hàn Quốc",EducationLevel="",MajorVn="",MajorKr="",
                                Marriage="Đã kết hôn",DependentChild="",DependentParents=""},
                            new Employee{DepartmentVn="Bộ phận hành chính",DepartmentKr="",Name="Trương Thị Sinh",EmployeeNo="CW0003",BankAccount="0731-000-687-102",BankLocation="VCB Bắc Giang",
                                TexNo ="8119415119",JobPosition="Nấu ăn",Sex="Nữ",BirthDate=DateTime.Parse("1985-07-20"),RegistrationNo="122137505",RegistrationDate=DateTime.Parse("2011-05-20"),
                                RegistrationPosition="CA Bắc Giang",Tel1="01693-242-713",Tel2="",Email="",
                                Adress="An Thịnh - Tiền Phong - Yên Dũng- Bắc Giang",People="Kinh",Religion="",Country="Việt Nam",EducationLevel="",MajorVn="",MajorKr="",
                                Marriage="Đã kết hôn",DependentChild="",DependentParents=""},
                            new Employee{DepartmentVn="Bộ phận hành chính",DepartmentKr="",Name="Nguyễn Thị Phích",EmployeeNo="CW0007",BankAccount="0731-000-687-103",BankLocation="VCB Bắc Giang",
                                TexNo ="8317114307",JobPosition="Nhân viên phụ bếp",Sex="Nữ",BirthDate=DateTime.Parse("1968-07-10"),RegistrationNo="121070099",RegistrationDate=DateTime.Parse("2012-10-06"),
                                RegistrationPosition="CA Bắc Giang",Tel1="01632-753-953",Tel2="",Email="",
                                Adress="Thôn Lực - Tân Mỹ - Bắc Giang",People="Kinh",Religion="",Country="Việt Nam",EducationLevel="",MajorVn="",MajorKr="",
                                Marriage="Đã kết hôn",DependentChild="",DependentParents=""},
                            new Employee{DepartmentVn="Bộ phận hành chính",DepartmentKr="",Name="Lê Thị Cầu",EmployeeNo="CW0038",BankAccount="0731000735851",BankLocation="VCB Bắc Giang",
                                TexNo ="8461765742",JobPosition="Nhân viên vệ sinh",Sex="Nữ",BirthDate=DateTime.Parse("1968-01-01"),RegistrationNo="121998148",RegistrationDate=DateTime.Parse("2008-04-27"),
                                RegistrationPosition="CA Bắc Giang",Tel1="0962-237-350",Tel2="01634-440-115",Email="",
                                Adress="Việt Thắng - Đồng Phúc - Yên Dũng - Bắc Giang",People="Kinh",Religion="",Country="Việt Nam",EducationLevel="",MajorVn="",MajorKr="",
                                Marriage="Đã kết hôn",DependentChild="",DependentParents=""},
                            new Employee{DepartmentVn="Bộ phận hành chính",DepartmentKr="",Name="Lý Thị Hải Yến",EmployeeNo="",BankAccount="0731-000-732-107",BankLocation="VCB Bắc Giang",
                                TexNo ="8251644124",JobPosition="Thời vụ bếp",Sex="Nữ",BirthDate=DateTime.Parse("1994-06-29"),RegistrationNo="122051524",RegistrationDate=DateTime.Parse("2012-02-07"),
                                RegistrationPosition="CA Bắc Giang",Tel1="01693-242-713",Tel2="",Email="",
                                Adress="Song Khê - Bắc Giang",People="Kinh",Religion="",Country="Việt Nam",EducationLevel="",MajorVn="",MajorKr="",
                                Marriage="Đã kết hôn",DependentChild="",DependentParents=""},
                            new Employee{DepartmentVn="Bộ phận hành chính",DepartmentKr="",Name="Ngô Thị Duyên",EmployeeNo="",BankAccount="0731-000-694-378",BankLocation="VCB Bắc Giang",
                                TexNo ="8363614424",JobPosition="NV phiên dịch",Sex="Nữ",BirthDate=DateTime.Parse("1993-06-07"),RegistrationNo="163260397",RegistrationDate=DateTime.Parse("2013-06-11"),
                                RegistrationPosition="CA Nam Định",Tel1="0979-006-493",Tel2="",Email="duyenktx67@gmail.com",
                                Adress="Xuân Phong - Xuân Trường - Nam Định",People="Kinh",Religion="",Country="Việt Nam",EducationLevel="Đại học",MajorVn="",MajorKr="",
                                Marriage="Độc thân",DependentChild="",DependentParents=""}

                        };
                        employees.ForEach(s => context.Employees.AddOrUpdate(p => p.Name, s));
            //            employees.ForEach(s => context.Employees.Add(s));
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
                            new Personnel{EmployeeID= employees.Single(e => e.Name == "Nguyễn Thị Phích").ID, SendDate=DateTime.Parse("2016-05-25"), Reason = "",
                                StartDate=DateTime.Parse("2016-05-25"),EndDate=DateTime.Parse("2016-05-25"),Type=0},
                            new Personnel{EmployeeID= employees.Single(e => e.Name == "Nguyễn Thị Phích").ID, SendDate=DateTime.Parse("2016-05-25"), Reason = "",
                                StartDate=DateTime.Parse("2016-06-25"),EndDate=DateTime.Parse("2016-06-25"),Type=1},
                            new Personnel{EmployeeID= employees.Single(e => e.Name == "Nguyễn Thị Phích").ID, SendDate=DateTime.Parse("2016-05-25"), Reason = "",
                                StartDate=DateTime.Parse("2016-07-25"),EndDate=DateTime.Parse("2016-07-25"),Type=2},
                            new Personnel{EmployeeID= employees.Single(e => e.Name == "Nguyễn Thị Phích").ID, SendDate=DateTime.Parse("2016-05-25"), Reason = "",
                                StartDate=DateTime.Parse("2016-08-25"),EndDate=DateTime.Parse("2016-08-25"),Type=3},
                            new Personnel{EmployeeID= employees.Single(e => e.Name == "Nguyễn Thị Phích").ID, SendDate=DateTime.Parse("2016-05-25"), Reason = "",
                                StartDate=DateTime.Parse("2016-09-25"),EndDate=DateTime.Parse("2016-09-25"),Type=4}
                            new Personnel{EmployeeID= employees.Single(e => e.Name == "Nguyễn Thị Phích").ID, SendDate=DateTime.Parse("2016-05-25"), Reason = "",
                                StartDate=DateTime.Parse("2016-09-25"),EndDate=DateTime.Parse("2016-09-25"),Type=5}
                            new Personnel{EmployeeID= employees.Single(e => e.Name == "Nguyễn Thị Phích").ID, SendDate=DateTime.Parse("2016-05-25"), Reason = "",
                                StartDate=DateTime.Parse("2016-09-25"),EndDate=DateTime.Parse("2016-09-25"),Type=6}
                        };
                        personnels.ForEach(s => context.Personnels.Add(s));
                        context.SaveChanges();

                        var resign = new List<Resign>
                        {
                            new Resign{EmployeeID= employees.Single(e => e.Name == "Ngô Thị Duyên").ID,
                                ResignDate=DateTime.Parse("2016-08-06"),ResignNoteVn="Đi du học",ResignNoteKr="한국유학"}
                        };
                        resign.ForEach(s => context.Resigns.Add(s));
                        context.SaveChanges();

                        var company = new List<Company>
                        {
                            new Company { Name = "Company A", Address = "AAA", Tel = "1111111111", Texcode = "11111111" },
                            new Company { Name = "Company B", Address = "BBB", Tel = "2222222222", Texcode = "22222222" },
                            new Company { Name = "Company C", Address = "CCC", Tel = "3333333333", Texcode = "33333333" }
                        };
                        company.ForEach(s => context.Companys.Add(s));
                        context.SaveChanges();

                        var project = new List<Project>
                        {
                            new Project { CompanyID = company.Single(e => e.Name == "Company A").ID, Name = "Project A", Date = DateTime.Parse("2016-08-06"), MaterialBuys = new List<MaterialBuy>() },
                            new Project { CompanyID = company.Single(e => e.Name == "Company B").ID, Name = "Project B", Date = DateTime.Parse("2017-01-10"), MaterialBuys = new List<MaterialBuy>() },
                            new Project { CompanyID = company.Single(e => e.Name == "Company C").ID, Name = "Project C", Date = DateTime.Parse("2017-08-16"), MaterialBuys = new List<MaterialBuy>() }
                        };
                        project.ForEach(s => context.Projects.AddOrUpdate(p => p.Name, s));
                        //            project.ForEach(s => context.Projects.Add(s));
                        context.SaveChanges();

                        var materialname = new List<MaterialName>
                        {
                            new MaterialName { Name = "Material A1", Sort = "AAA" },
                            new MaterialName { Name = "Material A2", Sort = "AAA" },
                            new MaterialName { Name = "Material B1", Sort = "BBB" },
                            new MaterialName { Name = "Material B2", Sort = "BBB" },
                            new MaterialName { Name = "Material C1", Sort = "CCC" },
                            new MaterialName { Name = "Material C2", Sort = "CCC" }
                        };
                        materialname.ForEach(s => context.MaterialNames.Add(s));
                        context.SaveChanges();

                        var materialunitprice = new List<MaterialUnitPrice>
                        {
                            new MaterialUnitPrice{ MaterialNameID = materialname.Single(e => e.Name == "Material A1").ID, CompanyID = company.Single(e => e.Name == "Company A").ID,
                                Date = DateTime.Parse("2016-08-06"), Unit = Unit.Quantity, Price = 1000000},
                            new MaterialUnitPrice{ MaterialNameID = materialname.Single(e => e.Name == "Material A2").ID, CompanyID = company.Single(e => e.Name == "Company A").ID,
                                Date = DateTime.Parse("2016-08-06"), Unit = Unit.Quantity, Price = 2000000},
                            new MaterialUnitPrice{ MaterialNameID = materialname.Single(e => e.Name == "Material B1").ID, CompanyID = company.Single(e => e.Name == "Company B").ID,
                                Date = DateTime.Parse("2016-08-06"), Unit = Unit.Quantity, Price = 3000000},
                            new MaterialUnitPrice{ MaterialNameID = materialname.Single(e => e.Name == "Material B2").ID, CompanyID = company.Single(e => e.Name == "Company B").ID,
                                Date = DateTime.Parse("2016-08-06"), Unit = Unit.Quantity, Price = 4000000},
                            new MaterialUnitPrice{ MaterialNameID = materialname.Single(e => e.Name == "Material C1").ID, CompanyID = company.Single(e => e.Name == "Company C").ID,
                                Date = DateTime.Parse("2016-08-06"), Unit = Unit.Quantity, Price = 5000000},
                            new MaterialUnitPrice{ MaterialNameID = materialname.Single(e => e.Name == "Material C2").ID, CompanyID = company.Single(e => e.Name == "Company C").ID,
                                Date = DateTime.Parse("2016-08-06"), Unit = Unit.Quantity, Price = 6000000}
                        };
                        materialunitprice.ForEach(s => context.MaterialUnitPrices.Add(s));
                        context.SaveChanges();

                        var materialbuyunit = new List<MaterialBuyUnit>
                        {
                            new MaterialBuyUnit{ MaterialUnitPriceID = materialunitprice.Single(e => e.Price == 1000000).ID, Quantity = 2},
                            new MaterialBuyUnit{ MaterialUnitPriceID = materialunitprice.Single(e => e.Price == 2000000).ID, Quantity = 3},
                            new MaterialBuyUnit{ MaterialUnitPriceID = materialunitprice.Single(e => e.Price == 3000000).ID, Quantity = 4},
                            new MaterialBuyUnit{ MaterialUnitPriceID = materialunitprice.Single(e => e.Price == 4000000).ID, Quantity = 5},
                            new MaterialBuyUnit{ MaterialUnitPriceID = materialunitprice.Single(e => e.Price == 5000000).ID, Quantity = 6},
                            new MaterialBuyUnit{ MaterialUnitPriceID = materialunitprice.Single(e => e.Price == 6000000).ID, Quantity = 7}
                        };
                        materialbuyunit.ForEach(s => context.MaterialBuyUnits.Add(s));
                        context.SaveChanges();

                        var materialbuy = new List<MaterialBuy>
                        {
                            new MaterialBuy{ CompanyID = company.Single(e => e.Name == "Company A").ID, ProjectID = project.Single(e => e.Name == "Project A").ID,
                                MaterialBuyUnits = new List<MaterialBuyUnit>(), Date =DateTime.Parse("2016-08-06"), Note = "", VAT = true},
                            new MaterialBuy{ CompanyID = company.Single(e => e.Name == "Company B").ID, ProjectID = project.Single(e => e.Name == "Project B").ID,
                                MaterialBuyUnits = new List<MaterialBuyUnit>(), Date =DateTime.Parse("2016-08-06"), Note = "", VAT = true},
                            new MaterialBuy{ CompanyID = company.Single(e => e.Name == "Company C").ID, ProjectID = project.Single(e => e.Name == "Project C").ID,
                                MaterialBuyUnits = new List<MaterialBuyUnit>(), Date =DateTime.Parse("2016-08-06"), Note = "", VAT = true}
                        };
                        materialbuy[0].MaterialBuyUnits.Add(materialbuyunit[0]);
                        materialbuy[0].MaterialBuyUnits.Add(materialbuyunit[1]);
                        materialbuy[1].MaterialBuyUnits.Add(materialbuyunit[2]);
                        materialbuy[1].MaterialBuyUnits.Add(materialbuyunit[3]);
                        materialbuy[2].MaterialBuyUnits.Add(materialbuyunit[4]);
                        materialbuy[2].MaterialBuyUnits.Add(materialbuyunit[5]);
                        materialbuy.ForEach(s => context.MaterialBuys.Add(s));
                        context.SaveChanges();


                        project[0].MaterialBuys.Add(materialbuy[0]);
                        project[1].MaterialBuys.Add(materialbuy[1]);
                        project[2].MaterialBuys.Add(materialbuy[2]);



                        var user = new List<User>
                        {
                            new User{ EmployeeID = 1, UserID = "hahaha", UserPassword ="1012", Security = Security.Admin},
                            new User{ EmployeeID = 2, UserID = "test1", UserPassword ="", Security = Security.Power},
                            new User{ EmployeeID = 3, UserID = "test2", UserPassword ="", Security =Security.Translation},
                            new User{ EmployeeID = 4, UserID = "test3", UserPassword ="", Security =Security.Personnel},
                            new User{ EmployeeID = 5, UserID = "test4", UserPassword ="", Security =Security.Public},
                        };
                        user.ForEach(s => context.Users.Add(s));
                        context.SaveChanges();
            */
        }
    }
}
