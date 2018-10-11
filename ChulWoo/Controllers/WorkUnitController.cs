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
using ChulWoo.Helper;
using PagedList;

namespace ChulWoo.Controllers
{
    public class WorkUnitController : BaseController
    {
        private ChulWooContext db = new ChulWooContext();

        // GET: WorkUnit
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

            var workUnits = db.WorkUnits.Include(w => w.Project).OrderByDescending(w => w.StartDate);

            if (!String.IsNullOrEmpty(searchString))
                workUnits = (IOrderedQueryable<WorkUnit>)workUnits.Where(p => p.NoteVn.Contains(searchString));

            if (translate == true)
                workUnits = (IOrderedQueryable<WorkUnit>)workUnits.Where(p => !p.Translate);

            return View(workUnits.ToPagedList(pageNumber, pageSize));
        }

        // GET: WorkUnit/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WorkUnit workUnit = await db.WorkUnits.FindAsync(id);
            if (workUnit == null)
            {
                return HttpNotFound();
            }
            return View(workUnit);
        }

        // GET: WorkUnit/Create
        public ActionResult Create()
        {
            ViewBag.ProjectID = new SelectList(db.Projects, "ID", "NameVn");
            return View();
        }

        // POST: WorkUnit/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,ProjectID,StartDate,EndDate,Complete,WorkNameVn,WorkNameKr,NoteVn,NoteKr,Translate")] WorkUnit workUnit)
        {
            if (ModelState.IsValid)
            {
                db.WorkUnits.Add(workUnit);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.ProjectID = new SelectList(db.Projects, "ID", "NameVn", workUnit.ProjectID);
            return View(workUnit);
        }

        // GET: WorkUnit/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WorkUnit workUnit = await db.WorkUnits.FindAsync(id);
            if (workUnit == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProjectID = new SelectList(db.Projects, "ID", "NameVn", workUnit.ProjectID);
            return View(workUnit);
        }

        // POST: WorkUnit/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,ProjectID,StartDate,EndDate,Complete,WorkNameVn,WorkNameKr,NoteVn,NoteKr,Translate")] WorkUnit workUnit)
        {
            if (ModelState.IsValid)
            {
                db.Entry(workUnit).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index", new { translate = Session["Translate"] });
            }
            ViewBag.ProjectID = new SelectList(db.Projects, "ID", "NameVn", workUnit.ProjectID);
            return View(workUnit);
        }

        // GET: WorkUnit/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WorkUnit workUnit = await db.WorkUnits.FindAsync(id);
            if (workUnit == null)
            {
                return HttpNotFound();
            }
            return View(workUnit);
        }

        // POST: WorkUnit/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            WorkUnit workUnit = await db.WorkUnits.FindAsync(id);
            db.WorkUnits.Remove(workUnit);
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
