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
            if(project.MaterialBuys != null)
                project.MaterialBuys = project.MaterialBuys.OrderByDescending(m => m.Date).ToList();

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
            if (project.Company != null && project.Constructor != null && ModelState.IsValid)
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

            if (Convert.ToInt32(Session["LoginUserSecurity"]) < Convert.ToInt32(Security.Translation))
                return RedirectToAction("Index", "Home");

            Project sproject = await db.Projects.FindAsync(project.ID);
            Company company = db.Companys.FirstOrDefault(c => c.Name.Equals(project.Company.Name));
            Company constructor = db.Companys.FirstOrDefault(c => c.Name.Equals(project.Constructor.Name));
            if (sproject != null && company != null && constructor != null && ModelState.IsValid)
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
                sproject.ConstructorID = constructor.ID;
                sproject.ManagerID = project.ManagerID;
                sproject.Translate = project.Translate;

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
                return RedirectToAction("EditAddDeposit", new { id = projectDepositData.Porject.ID });
            }
            return View(projectDepositData);
        }

        public async Task<ActionResult> DeleteDeposit(int id, int depositid)
        {
            Payment Payment = await db.Payments.FindAsync(depositid);

            db.Payments.Remove(Payment);

            //PreDeleteUnit(id, paymentid);
            await db.SaveChangesAsync();
            return RedirectToAction("EditAddDeposit", new { id = id });
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
                return RedirectToAction("EditAddDeposit", new { id = projectDepositData.Porject.ID });
            }
            return View(projectDepositData);
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
