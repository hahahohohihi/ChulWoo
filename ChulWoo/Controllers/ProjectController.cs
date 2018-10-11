using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ChulWoo.DAL;
using ChulWoo.Models;
using ChulWoo.Viewmodel;
using System.IO;
using PagedList;

namespace ChulWoo.Controllers
{
    public class ProjectController : BaseController
    {
        private ChulWooContext db = new ChulWooContext();

        // GET: Project
        public async Task<ActionResult> Index(int? page, string currentFilter, string searchString, bool? translate)
        {
            if (Session["LoginUserID"] == null)
                return RedirectToAction("Login", "Account");

            int pageSize = 10;
            int pageNumber = (page ?? 1);

            if (searchString != null)
                page = 1;
            else
                searchString = currentFilter;

            ViewBag.CurrentFilter = searchString;
            Session["Translate"] = translate;

            var projects = db.Projects.Include(p => p.Company)
                .Include(p => p.MaterialBuys.Select(mb => mb.Project))
                .OrderByDescending(p => p.Date);

            if (!String.IsNullOrEmpty(searchString))
                projects = (IOrderedQueryable<Project>)projects.Where(p => p.NameKr.Contains(searchString) || p.NameVn.Contains(searchString));

            if (translate == true)
                projects = (IOrderedQueryable<Project>)projects.Where(m => !m.Translate);

            //            var projects = db.Projects.Include(p => p.Company);
            return View(projects.ToPagedList(pageNumber, pageSize));
        }

        // GET: Project/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (Session["LoginUserID"] == null)
                return RedirectToAction("Login", "Account");

            if (Convert.ToInt32(Session["LoginUserSecurity"]) < Convert.ToInt32(Security.Personnel))
                return RedirectToAction("Index", "Home");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //            Project project = await db.Projects.FindAsync(id);

            var project = db.Projects.Include(p => p.Company)
                .Include(p => p.MaterialBuys.Select(mb => mb.Project))
                .Include(p => p.WorkUnits.Select(w => w.Project))
                .Include(p => p.Deposits.Select(w => w.Project))
                .Where(p => p.ID == id)
                .Single();

//            project.Deposits = (ICollection<Payment>)db.Payments.Where(p => p.ProjectID == id && p.StatementType == Models.StatementType.Deposit).ToList();

            if (project == null)
            {
                return HttpNotFound();
            }

            double tPrice = 0, tVAT = 0, tPayment = 0, tDeposit = 0;
            if(project.MaterialBuys != null)
            {
                project.MaterialBuys = project.MaterialBuys.OrderByDescending(m => m.Date).ToList();

                foreach( var item in project.MaterialBuys)
                {
                    if( item.MaterialBuyType == MaterialBuyType.MaterialBuy )
                    {
                        foreach (var unit in item.MaterialBuyUnits)
                        {
                            var price = ((double)unit.Quantity * unit.MaterialUnitPrice.Price);
                            tPrice += price;
                            tVAT += price * unit.MaterialBuy.VATPer;
                        }
                        foreach (var unit in item.Payments)
                        {
                            if (unit.StatementType != Models.StatementType.Deposit)
                            {
                                tPayment += unit.Amount;
                            }
                        }
                    }
                }
            }

            if (project.Deposits != null)
            {
                foreach (var item in project.Deposits)
                {
                    if (item.StatementType == Models.StatementType.Deposit)
                    {
                        tDeposit += item.Amount;
                    }
                }
            }
            ViewBag.TotalPrice = tPrice;
            ViewBag.TotalVAT = tVAT;
            ViewBag.TotalPayment = tPayment;
            ViewBag.TotalDeposit = tDeposit;


            return View(project);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Details([Bind(Include = "ID,CompanyID,NameVn,NameKr,Date,Company,Manager,ManagerID,Constructor,ConstructorID,UploadFiles,Translate")] Project project)
        {
            if (Session["LoginUserID"] == null)
                return RedirectToAction("Login", "Account");

            if (Convert.ToInt32(Session["LoginUserSecurity"]) < Convert.ToInt32(Security.Manager))
                return RedirectToAction("Index", "Home");

            Project sproject = await db.Projects.FindAsync(project.ID);
            if (sproject != null && ModelState.IsValid)
            {
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    var file = Request.Files[i];

                    if (file != null && file.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(file.FileName);
                        UploadFile uploadFile = new UploadFile()
                        {
                            FileName = fileName,
                            SaveFileName = Guid.NewGuid() + "_" + fileName,
                            FolderName = "Project",
                            Date = DateTime.Now,
                            Security = true
                        };
                        var path = Path.Combine(Server.MapPath("~/UploadFile/"), uploadFile.FolderName + "/" + uploadFile.SaveFileName);
                        file.SaveAs(path);

                        db.Entry(uploadFile).State = EntityState.Added;

                        sproject.UploadFiles.Add(uploadFile);
                    }
                }

                await db.SaveChangesAsync();
                //                return RedirectToAction("Index");
                return RedirectToAction("Details", new { id = project.ID });
            }

            //            ViewBag.CompanyID = new SelectList(db.Companys, "ID", "Name", project.CompanyID);
            return View(project);
        }


        public async Task<ActionResult> DetailsWorkProcess(int? id, int? startYear, int? startMonth )
        {
            if (Session["LoginUserID"] == null)
                return RedirectToAction("Login", "Account");

            if (Convert.ToInt32(Session["LoginUserSecurity"]) < Convert.ToInt32(Security.Personnel))
                return RedirectToAction("Index", "Home");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //            Project project = await db.Projects.FindAsync(id);

            var project = db.Projects.Include(p => p.Company)
                .Include(p => p.MaterialBuys.Select(mb => mb.Project))
                .Include(p => p.WorkUnits.Select(w => w.Project))
                .Where(p => p.ID == id)
                .Single();

            project.Deposits = (ICollection<Payment>)db.Payments.Where(p => p.ProjectID == id && p.StatementType == Models.StatementType.Deposit).ToList();

            if (project == null)
            {
                return HttpNotFound();
            }
            if (project.MaterialBuys != null)
                project.MaterialBuys = project.MaterialBuys.OrderByDescending(m => m.Date).ToList();

            if( startYear == null)
            {
                ViewBag.startYear = DateTime.Now.Year;
                ViewBag.startMonth = DateTime.Now.Month;
            }
            else
            {
                ViewBag.startYear = startYear;
                ViewBag.startMonth = startMonth;
            }

            return View(project);
        }

        public async Task<ActionResult> DetailsDailyWork(int? id)
        {
            if (Session["LoginUserID"] == null)
                return RedirectToAction("Login", "Account");

            if (Convert.ToInt32(Session["LoginUserSecurity"]) < Convert.ToInt32(Security.Personnel))
                return RedirectToAction("Index", "Home");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //            Project project = await db.Projects.FindAsync(id);

            var project = db.Projects.Include(p => p.Company)
                .Include(p => p.MaterialBuys.Select(mb => mb.Project))
                .Include(p => p.WorkUnits.Select(w => w.Project))
                .Where(p => p.ID == id)
                .Single();

            project.Deposits = (ICollection<Payment>)db.Payments.Where(p => p.ProjectID == id && p.StatementType == Models.StatementType.Deposit).ToList();

            if (project == null)
            {
                return HttpNotFound();
            }
            if (project.MaterialBuys != null)
                project.MaterialBuys = project.MaterialBuys.OrderByDescending(m => m.Date).ToList();

            return View(project);
        }

        // GET: Project/Create
        public ActionResult Create()
        {
            if (Session["LoginUserID"] == null)
                return RedirectToAction("Login", "Account");

            if (Convert.ToInt32(Session["LoginUserSecurity"]) < Convert.ToInt32(Security.Translation))
                return RedirectToAction("Index", "Home");

//            ViewBag.CompanyID = new SelectList(db.Companys, "ID", "Name");
            ViewBag.ManagerID = new SelectList(db.Employees, "ID", "Name");
            return View();
        }

        // POST: Project/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,CompanyID,NameVn,NameKr,Date,Company,Constructor,ConstructorID,Manager,ManagerID,UploadFiles,Translate")] Project project)
        {
            if (Session["LoginUserID"] == null)
                return RedirectToAction("Login", "Account");

            if (Convert.ToInt32(Session["LoginUserSecurity"]) < Convert.ToInt32(Security.Translation))
                return RedirectToAction("Index", "Home");

            project.Company = db.Companys.FirstOrDefault(c => c.Name.Equals(project.Company.Name));
            project.Constructor = db.Companys.FirstOrDefault(c => c.Name.Equals(project.Constructor.Name));
            if (project.Company != null && ModelState.IsValid)
            {
                List<UploadFile> UploadFiles = new List<UploadFile>();
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    var file = Request.Files[i];

                    if (file != null && file.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(file.FileName);
                        UploadFile uploadFile = new UploadFile()
                        {
                            FileName = fileName,
                            SaveFileName = Guid.NewGuid() + "_" + fileName,
                            FolderName = "Project",
                            Date = DateTime.Now
                        };
                        UploadFiles.Add(uploadFile);

                        var path = Path.Combine(Server.MapPath("~/UploadFile/"), uploadFile.FolderName + "/" + uploadFile.SaveFileName);
                        file.SaveAs(path);
                    }
                }

                project.UploadFiles = UploadFiles;
                project.CompanyID = project.Company.ID;
                if( project.Constructor != null )
                    project.ConstructorID = project.Constructor.ID;
                DateTime date = (DateTime)project.Date;
                project.Date = new DateTime(date.Year, date.Month, date.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                db.Projects.Add(project);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

//            ViewBag.CompanyID = new SelectList(db.Companys, "ID", "Name", project.CompanyID);
            return View(project);
        }

        // GET: Project/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (Session["LoginUserID"] == null)
                return RedirectToAction("Login", "Account");

            if (Convert.ToInt32(Session["LoginUserSecurity"]) < Convert.ToInt32(Security.Personnel))
                return RedirectToAction("Index", "Home");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = await db.Projects.FindAsync(id);
            if (project == null)
            {
                return HttpNotFound();
            }
//            ViewBag.CompanyID = new SelectList(db.Companys, "ID", "Name", project.CompanyID);
            ViewBag.ManagerID = new SelectList(db.Employees, "ID", "Name", project.ManagerID);
            return View(project);
        }

        // POST: Project/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,CompanyID,NameVn,NameKr,Date,Company,Manager,ManagerID,Constructor,ConstructorID,UploadFiles,Translate")] Project project)
        {
            if (Session["LoginUserID"] == null)
                return RedirectToAction("Login", "Account");

            if (Convert.ToInt32(Session["LoginUserSecurity"]) < Convert.ToInt32(Security.Manager))
                return RedirectToAction("Index", "Home");

            Project sproject = await db.Projects.FindAsync(project.ID);
            Company company = db.Companys.FirstOrDefault(c => c.Name.Equals(project.Company.Name));
            Company constructor = db.Companys.FirstOrDefault(c => c.Name.Equals(project.Constructor.Name));
            if (sproject != null && company != null && ModelState.IsValid)
            {
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    var file = Request.Files[i];

                    if (file != null && file.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(file.FileName);
                        UploadFile uploadFile = new UploadFile()
                        {
                            FileName = fileName,
                            SaveFileName = Guid.NewGuid() + "_" + fileName,
                            FolderName = "Project",
                            Date = DateTime.Now
                        };
                        var path = Path.Combine(Server.MapPath("~/UploadFile/"), uploadFile.FolderName + "/" + uploadFile.SaveFileName);
                        file.SaveAs(path);

                        db.Entry(uploadFile).State = EntityState.Added;

                        sproject.UploadFiles.Add(uploadFile);
                    }
                }

                sproject.CompanyID = company.ID;
                sproject.Company = company;
                sproject.Date = project.Date;
                sproject.NameVn = project.NameVn;
                sproject.NameKr = project.NameKr;
                sproject.ManagerID = project.ManagerID;
                sproject.Translate = project.Translate;

                if( constructor != null )
                    sproject.ConstructorID = constructor.ID;

                company.Projects.Add(sproject);
                db.Entry(sproject).State = EntityState.Modified;
                await db.SaveChangesAsync();
//                return RedirectToAction("Index");
                return RedirectToAction("Index", new { translate = Session["Translate"] });
            }

            //            ViewBag.CompanyID = new SelectList(db.Companys, "ID", "Name", project.CompanyID);
            return View(project);
        }

        // GET: Project/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (Session["LoginUserID"] == null)
                return RedirectToAction("Login", "Account");

            if (Convert.ToInt32(Session["LoginUserSecurity"]) < Convert.ToInt32(Security.Translation))
                return RedirectToAction("Index", "Home");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = await db.Projects.FindAsync(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: Project/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            if (Session["LoginUserID"] == null)
                return RedirectToAction("Login", "Account");

            if (Convert.ToInt32(Session["LoginUserSecurity"]) < Convert.ToInt32(Security.Translation))
                return RedirectToAction("Index", "Home");

            try
            {
                Project project = await db.Projects.FindAsync(id);
                if (project == null)
                {
                    return HttpNotFound();
                }

                //delete files from the file system

                foreach (var item in project.UploadFiles)
                {
                    String path = Path.Combine(Server.MapPath("~/UploadFile/"), item.FolderName + "/" + item.SaveFileName);
                    if (System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);
                    }
                }

                db.Projects.Remove(project);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        public async Task<ActionResult> EditAddDeposit(int? id)
        {
            if (Session["LoginUserID"] == null)
                return RedirectToAction("Login", "Account");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = await db.Projects.FindAsync(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            var projectDeposit = new ProjectDepositData();
            projectDeposit.Porject = project;

            return View(projectDeposit);
        }

        // POST: MaterialBuy/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAddDeposit(ProjectDepositData projectDepositData)
        {
            if (Session["LoginUserID"] == null)
                return RedirectToAction("Login", "Account");

            if (ModelState.IsValid)
            {
                //                db.Entry(materialBuyData).State = EntityState.Modified;
                Project project = db.Projects.FirstOrDefault(m => m.ID == projectDepositData.Porject.ID);

                projectDepositData.Deposit.EmployeeID = Convert.ToInt32(Session["LoginUserEmployeeID"]);
                projectDepositData.Deposit.Employee = db.Employees.FirstOrDefault(e => e.ID == projectDepositData.Deposit.EmployeeID);
                projectDepositData.Deposit.StatementType = Models.StatementType.Deposit;
                projectDepositData.Deposit.CompanyID = projectDepositData.Porject.CompanyID;
                projectDepositData.Deposit.ProjectID = projectDepositData.Porject.ID;

                db.Payments.Add(projectDepositData.Deposit);
                project.Deposits.Add(projectDepositData.Deposit);

                await db.SaveChangesAsync();
                return RedirectToAction("Details", new { id = projectDepositData.Porject.ID });
            }
            return View(projectDepositData);
        }

        public async Task<ActionResult> DeleteDeposit(int id, int depositid)
        {
            Payment Payment = await db.Payments.FindAsync(depositid);

            db.Payments.Remove(Payment);

            //PreDeleteUnit(id, paymentid);
            await db.SaveChangesAsync();
            return RedirectToAction("Details", new { id = id });
        }

        public async Task<ActionResult> EditEditDeposit(int? id, int depositid)
        {
            if (Session["LoginUserID"] == null)
                return RedirectToAction("Login", "Account");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = await db.Projects.FindAsync(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            Payment deposit = await db.Payments.FindAsync(depositid);
            if (deposit == null)
            {
                return HttpNotFound();
            }
            var projectDeposit = new ProjectDepositData();
            projectDeposit.Porject = project;
            projectDeposit.Deposit = deposit;

            projectDeposit.Porject.Deposits = projectDeposit.Porject.Deposits.OrderBy(p => p.Date).ToList();


            return View(projectDeposit);
        }

        // POST: MaterialBuy/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditEditDeposit(int id, int depositid, ProjectDepositData projectDepositData)
        {
            if (Session["LoginUserID"] == null)
                return RedirectToAction("Login", "Account");

            if (ModelState.IsValid)
            {
                //                db.Entry(materialBuyData).State = EntityState.Modified;
                Project project = db.Projects.FirstOrDefault(m => m.ID == projectDepositData.Porject.ID);
                Payment payment = await db.Payments.FindAsync(depositid);

                payment.Date = projectDepositData.Deposit.Date;
                payment.Type = projectDepositData.Deposit.Type;
                payment.Amount = projectDepositData.Deposit.Amount;
                payment.CompanyID = projectDepositData.Porject.CompanyID;
                payment.ProjectID = projectDepositData.Porject.ID;

                if (payment.NoteVn != null && !payment.NoteVn.Equals(projectDepositData.Deposit.NoteVn))
                {
                    payment.NoteVn = projectDepositData.Deposit.NoteVn;
                    payment.Translate = false;
                }
                if (projectDepositData.Deposit.NoteVn != null)
                {
                    payment.NoteVn = projectDepositData.Deposit.NoteVn;
                    payment.Translate = false;
                }
                if (payment.NoteVn == null || payment.NoteVn.Length <= 0)
                    payment.Translate = true;

                await db.SaveChangesAsync();
                return RedirectToAction("Details", new { id = projectDepositData.Porject.ID });
            }
            return View(projectDepositData);
        }

        public async Task<ActionResult> EditAddMakeReceipt(int? id)
        {
            if (Session["LoginUserID"] == null)
                return RedirectToAction("Login", "Account");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = await db.Projects.FindAsync(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            var projectMakeReceiptData = new ProjectMakeReceiptData();
            projectMakeReceiptData.Porject = project;

            return View(projectMakeReceiptData);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAddMakeReceipt(ProjectMakeReceiptData projectMakeReceiptData)
        {
            if (Session["LoginUserID"] == null)
                return RedirectToAction("Login", "Account");

            if (ModelState.IsValid)
            {

                //                db.Entry(materialBuyData).State = EntityState.Modified;
                Project project = db.Projects.FirstOrDefault(m => m.ID == projectMakeReceiptData.Porject.ID);

                MaterialBuy receipt = new MaterialBuy();
                receipt.Company = project.Company;
                receipt.CompanyID = project.CompanyID;
                receipt.Date = projectMakeReceiptData.Date;
                receipt.EmployeeID = Convert.ToInt32(Session["LoginUserEmployeeID"]);
                receipt.Employee = db.Employees.FirstOrDefault(e => e.ID == receipt.EmployeeID);
                receipt.MaterialBuyType = MaterialBuyType.MakeReceipt;
                receipt.NoteVn = projectMakeReceiptData.NoteVn;
                receipt.NoteKr = projectMakeReceiptData.NoteVn;
                receipt.Project = project;
                receipt.ProjectID = project.ID;
                receipt.VATPer = projectMakeReceiptData.VATPer;

                MaterialBuyUnit unit = new MaterialBuyUnit();
                unit.MaterialBuy = receipt;
                unit.Quantity = 1;


                MaterialName mName = db.MaterialNames.FirstOrDefault(m => m.NameVn.Equals(projectMakeReceiptData.NameVn) ||
                    m.NameKr.Equals(projectMakeReceiptData.NameVn));
                if( mName == null)
                {
                    mName = new MaterialName();
                    mName.NameKr = projectMakeReceiptData.NameVn;
                    mName.NameVn = projectMakeReceiptData.NameVn;
                    db.MaterialNames.Add(mName);
                }

                MaterialUnitPrice muPrice = db.MaterialUnitPrices.OrderBy(m => m.Date)
                    .FirstOrDefault(mp => mp.MaterialName.NameVn.Equals(projectMakeReceiptData.NameVn) ||
                    mp.MaterialName.NameKr.Equals(projectMakeReceiptData.NameVn));

                if (muPrice == null || muPrice.Price != projectMakeReceiptData.Price)
                {
                    muPrice = new MaterialUnitPrice();
                    muPrice.Company = project.Company;
                    muPrice.CompanyID = project.CompanyID;
                    muPrice.Date = projectMakeReceiptData.Date;
                    muPrice.UnitString = "Set";
                    muPrice.Price = projectMakeReceiptData.Price;

                    db.MaterialUnitPrices.Add(muPrice);
                }

                muPrice.MaterialName = mName;
                unit.MaterialUnitPrice = muPrice;
                receipt.MaterialBuyUnits = new List<MaterialBuyUnit>();
                receipt.MaterialBuyUnits.Add(unit);

                db.MaterialBuys.Add(receipt);
                db.MaterialBuyUnits.Add(unit);
                project.MaterialBuys.Add(receipt);

//                db.Payments.Add(projectDepositData.Deposit);
//                project.Deposits.Add(projectDepositData.Deposit);

                await db.SaveChangesAsync();
                return RedirectToAction("EditAddMakeReceipt", new { id = projectMakeReceiptData.Porject.ID });
            }
            return View(projectMakeReceiptData);
        }

        public async Task<ActionResult> EditEditMakeReceipt(int? id, int makereceiptid)
        {
            if (Session["LoginUserID"] == null)
                return RedirectToAction("Login", "Account");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = await db.Projects.FindAsync(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            MaterialBuy makeReceipt = await db.MaterialBuys.FindAsync(makereceiptid);
            if (makeReceipt == null)
            {
                return HttpNotFound();
            }
            var projectMakeReceiptData = new ProjectMakeReceiptData();
            projectMakeReceiptData.Porject = project;
            projectMakeReceiptData.Date = makeReceipt.Date;
            projectMakeReceiptData.NoteVn = makeReceipt.NoteVn;
            projectMakeReceiptData.NameVn = makeReceipt.MaterialBuyUnits.FirstOrDefault().MaterialUnitPrice.MaterialName.NameVn;
            projectMakeReceiptData.Price = makeReceipt.MaterialBuyUnits.FirstOrDefault().MaterialUnitPrice.Price;
            projectMakeReceiptData.VATPer = makeReceipt.VATPer;

            ViewBag.MakeReceiptID = makereceiptid;

            projectMakeReceiptData.Porject.MaterialBuys = projectMakeReceiptData.Porject.MaterialBuys.OrderByDescending(p => p.Date).ToList();

            return View(projectMakeReceiptData);
        }

        // POST: MaterialBuy/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditEditMakeReceipt(int id, int makereceiptid, ProjectMakeReceiptData projectMakeReceiptData)
        {
            if (Session["LoginUserID"] == null)
                return RedirectToAction("Login", "Account");

            if (ModelState.IsValid)
            {
                //                db.Entry(materialBuyData).State = EntityState.Modified;
                Project project = db.Projects.FirstOrDefault(m => m.ID == projectMakeReceiptData.Porject.ID);
                MaterialBuy makeReceipt = await db.MaterialBuys.FindAsync(makereceiptid);


                if (makeReceipt.Date != projectMakeReceiptData.Date)
                {
                    makeReceipt.Date = projectMakeReceiptData.Date;
                    makeReceipt.MaterialBuyUnits.FirstOrDefault().MaterialUnitPrice.Date = projectMakeReceiptData.Date;
                }

                if (makeReceipt.VATPer != projectMakeReceiptData.VATPer)
                {
                    makeReceipt.VATPer = projectMakeReceiptData.VATPer;
                }

                if ((makeReceipt.NoteVn != null && !makeReceipt.NoteVn.Equals(projectMakeReceiptData.NoteVn)) ||
                    (makeReceipt.NoteVn == null && projectMakeReceiptData.NoteVn != null) )
                {
                    makeReceipt.NoteVn = projectMakeReceiptData.NoteVn;
                    makeReceipt.NoteKr = projectMakeReceiptData.NoteVn;
                    makeReceipt.Translate = false;
                }

                if(makeReceipt.MaterialBuyUnits.FirstOrDefault().MaterialUnitPrice.Price != projectMakeReceiptData.Price )
                {
                    makeReceipt.MaterialBuyUnits.FirstOrDefault().MaterialUnitPrice.Price = projectMakeReceiptData.Price;
                }

                if ((projectMakeReceiptData.NameVn != null && !projectMakeReceiptData.NameVn.Equals(makeReceipt.MaterialBuyUnits.FirstOrDefault().MaterialUnitPrice.MaterialName.NameVn)) ||
                    (projectMakeReceiptData.NameVn == null && makeReceipt.MaterialBuyUnits.FirstOrDefault().MaterialUnitPrice.MaterialName.NameVn !=null) )
                {
                    int nameCount = db.MaterialUnitPrices.Where(m => m.MaterialName.NameVn.Equals(makeReceipt.MaterialBuyUnits.FirstOrDefault().MaterialUnitPrice.MaterialName.NameVn)).Count();
                    MaterialName oldName = db.MaterialNames.FirstOrDefault(m => m.NameVn.Equals(makeReceipt.MaterialBuyUnits.FirstOrDefault().MaterialUnitPrice.MaterialName.NameVn) ||
                        m.NameKr.Equals(makeReceipt.MaterialBuyUnits.FirstOrDefault().MaterialUnitPrice.MaterialName.NameVn));
                    MaterialName mName = db.MaterialNames.FirstOrDefault(m => m.NameVn.Equals(projectMakeReceiptData.NameVn) ||
                        m.NameKr.Equals(projectMakeReceiptData.NameVn));
                    if (mName == null)
                    {
                        mName = new MaterialName();
                        mName.NameKr = projectMakeReceiptData.NameVn;
                        mName.NameVn = projectMakeReceiptData.NameVn;
                        makeReceipt.MaterialBuyUnits.FirstOrDefault().MaterialUnitPrice.MaterialName = mName;
                        db.MaterialNames.Add(mName);
                    }
                    else
                    {
                        makeReceipt.MaterialBuyUnits.FirstOrDefault().MaterialUnitPrice.MaterialName = mName;
                    }

                    if( nameCount == 1 )
                        db.MaterialNames.Remove(oldName);
                }

                await db.SaveChangesAsync();
                return RedirectToAction("EditAddMakeReceipt", new { id = projectMakeReceiptData.Porject.ID });
            }
            return View(projectMakeReceiptData);
        }

        public async Task<ActionResult> DeleteMakeReceipt(int id, int makereceiptid)
        {
            if (Session["LoginUserID"] == null)
                return RedirectToAction("Login", "Account");

            if (makereceiptid == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MaterialBuy materialBuy = await db.MaterialBuys.FindAsync(makereceiptid);
            if (materialBuy == null)
            {
                return HttpNotFound();
            }

            if (materialBuy.MaterialBuyUnits.Count > 0)
            {
                var mtu = db.MaterialBuyUnits.Where(m => m.MaterialBuyID == makereceiptid).ToArray();

                foreach (var item in mtu)
                {
                    //db.MaterialBuyUnits.Remove(item);
                    //await DeleteUnit(materialBuy.ID, item.ID);
                    PreDeleteUnit(materialBuy.ID, item.ID);
                }
            }

            if (materialBuy.Payments.Count > 0)
            {
                var mtp = db.Payments.Where(p => p.MaterialBuys.Count(m => m.ID == makereceiptid) > 0).ToArray();

                foreach (var item in mtp)
                {
                    if (item.MaterialBuys.Count() > 1)
                    {
                        foreach (var mb in item.MaterialBuys)
                        {
                            if (mb.ID == makereceiptid)
                                item.MaterialBuys.Remove(mb);
                        }

                    }
                    else
                        db.Payments.Remove(item);
                }
            }

            db.MaterialBuys.Remove(materialBuy);
            await db.SaveChangesAsync();
            return RedirectToAction("Details", new { id = id });
        }

        public void PreDeleteUnit(int id, int unitid)
        {
            MaterialBuyUnit materialBuyUnit = db.MaterialBuyUnits.Find(unitid);
            int unitcount = db.MaterialBuyUnits.Count(m => m.MaterialUnitPriceID == materialBuyUnit.MaterialUnitPriceID);
            if (unitcount <= 1)
            {
                int namecount = db.MaterialUnitPrices.Count(m => m.MaterialNameID == materialBuyUnit.MaterialUnitPrice.MaterialNameID);
                if (namecount <= 1)
                    db.MaterialNames.Remove(materialBuyUnit.MaterialUnitPrice.MaterialName);

                db.MaterialUnitPrices.Remove(materialBuyUnit.MaterialUnitPrice);
            }

            db.MaterialBuyUnits.Remove(materialBuyUnit);
        }

        public async Task<ActionResult> AddAllDeposit(int id, int makereceiptid)
        {
            if (Session["LoginUserID"] == null)
                return RedirectToAction("Login", "Account");

            if (Convert.ToInt32(Session["LoginUserSecurity"]) < Convert.ToInt32(Security.Personnel))
                return RedirectToAction("Index", "Home");

            if (id == null || makereceiptid == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MaterialBuy makeReceipt = await db.MaterialBuys.FindAsync(makereceiptid);
            if (makeReceipt == null)
            {
                return HttpNotFound();
            }

            var paymentsum = makeReceipt.Payments.Sum(p => p.Amount);
            var price = makeReceipt.MaterialBuyUnits.Sum(m => m.Quantity * m.MaterialUnitPrice.Price);
            var amount = price + price * makeReceipt.VATPer - paymentsum;
            if (amount > 0)
            {
                Payment payment = new Payment();
                payment.CompanyID = makeReceipt.CompanyID;
                payment.Date = DateTime.Now;
                payment.EmployeeID = Convert.ToInt32(Session["LoginUserEmployeeID"]);
                payment.ProjectID = makeReceipt.ProjectID;
                payment.StatementType = Models.StatementType.Deposit;
                payment.Type = PaymentType.Bank;
                payment.Amount = (double)amount;

                db.Payments.Add(payment);
                makeReceipt.Payments.Add(payment);

                await db.SaveChangesAsync();
            }

            return RedirectToAction("Details", new { id = id });
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
