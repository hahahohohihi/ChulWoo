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
            ViewBag.translate = translate;

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

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //            Project project = await db.Projects.FindAsync(id);

            var project = db.Projects.Include(p => p.Company)
                .Include(p => p.MaterialBuys.Select(mb => mb.Project))
                .Where(p => p.ID == id)
                .Single();

            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // GET: Project/Create
        public ActionResult Create()
        {
            if (Session["LoginUserID"] == null)
                return RedirectToAction("Login", "Account");

            ViewBag.CompanyID = new SelectList(db.Companys, "ID", "Name");
            return View();
        }

        // POST: Project/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,CompanyID,NameVn,NameKr,Date,Company,UploadFiles,Translate")] Project project)
        {
            if (Session["LoginUserID"] == null)
                return RedirectToAction("Login", "Account");

            project.Company = db.Companys.FirstOrDefault(c => c.Name.Equals(project.Company.Name));
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

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = await db.Projects.FindAsync(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            ViewBag.CompanyID = new SelectList(db.Companys, "ID", "Name", project.CompanyID);
            return View(project);
        }

        // POST: Project/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,CompanyID,NameVn,NameKr,Date,Company,UploadFiles,Translate")] Project project)
        {
            if (Session["LoginUserID"] == null)
                return RedirectToAction("Login", "Account");

            Project sproject = await db.Projects.FindAsync(project.ID);
            Company company = db.Companys.FirstOrDefault(c => c.Name.Equals(project.Company.Name));
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
                sproject.Translate = project.Translate;

                company.Projects.Add(sproject);
                db.Entry(sproject).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

//            ViewBag.CompanyID = new SelectList(db.Companys, "ID", "Name", project.CompanyID);
            return View(project);
        }

        // GET: Project/Delete/5
        public async Task<ActionResult> Delete(int? id)
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
            return View(project);
        }

        // POST: Project/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            if (Session["LoginUserID"] == null)
                return RedirectToAction("Login", "Account");

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
