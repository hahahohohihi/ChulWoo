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

namespace ChulWoo.Controllers
{
    public class ProjectController : BaseController
    {
        private ChulWooContext db = new ChulWooContext();

        // GET: Project
        public async Task<ActionResult> Index()
        {
            var projects = db.Projects.Include(p => p.Company)
                .Include(p => p.MaterialBuys.Select(mb => mb.Project))
                .OrderBy(p => p.Date);
//            var projects = db.Projects.Include(p => p.Company);
            return View(await projects.ToListAsync());
        }

        // GET: Project/Details/5
        public async Task<ActionResult> Details(int? id)
        {
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
            ViewBag.CompanyID = new SelectList(db.Companys, "ID", "Name");
            return View();
        }

        // POST: Project/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,CompanyID,Name,Date")] Project project)
        {
            if (ModelState.IsValid)
            {
                db.Projects.Add(project);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.CompanyID = new SelectList(db.Companys, "ID", "Name", project.CompanyID);
            return View(project);
        }

        // GET: Project/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
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
        public async Task<ActionResult> Edit([Bind(Include = "ID,CompanyID,Name,Date")] Project project)
        {
            if (ModelState.IsValid)
            {
                db.Entry(project).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.CompanyID = new SelectList(db.Companys, "ID", "Name", project.CompanyID);
            return View(project);
        }

        // GET: Project/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
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
            Project project = await db.Projects.FindAsync(id);
            db.Projects.Remove(project);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
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
