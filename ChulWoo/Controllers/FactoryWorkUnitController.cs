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
    public class FactoryWorkUnitController : BaseController
    {
        private ChulWooContext db = new ChulWooContext();

        // GET: FactoryWorkUnit
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

            var factoryWorkUnits = db.FactoryWorkUnits.Include(f => f.FactoryDailyWork).Include(f => f.Project)
                .OrderByDescending(p => p.ID);

            if (!String.IsNullOrEmpty(searchString))
                factoryWorkUnits = (IOrderedQueryable<FactoryWorkUnit>)factoryWorkUnits.Where(p => p.NoteVn.Contains(searchString));

            if (translate == true)
                factoryWorkUnits = (IOrderedQueryable<FactoryWorkUnit>)factoryWorkUnits.Where(p => !p.Translate);

            return View(factoryWorkUnits.ToPagedList(pageNumber, pageSize));
        }

        // GET: FactoryWorkUnit/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FactoryWorkUnit factoryWorkUnit = await db.FactoryWorkUnits.FindAsync(id);
            if (factoryWorkUnit == null)
            {
                return HttpNotFound();
            }
            return View(factoryWorkUnit);
        }

        // GET: FactoryWorkUnit/Create
        public ActionResult Create()
        {
            ViewBag.FactoryDailyWorkID = new SelectList(db.FactoryDailyWorks, "ID", "NoteVn");
            ViewBag.ProjectID = new SelectList(db.Projects, "ID", "NameVn");
            return View();
        }

        // POST: FactoryWorkUnit/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,FactoryDailyWorkID,ProjectID,Type,EquipCount,NoteVn,NoteKr,Translate")] FactoryWorkUnit factoryWorkUnit)
        {
            if (ModelState.IsValid)
            {
                db.FactoryWorkUnits.Add(factoryWorkUnit);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.FactoryDailyWorkID = new SelectList(db.FactoryDailyWorks, "ID", "NoteVn", factoryWorkUnit.FactoryDailyWorkID);
            ViewBag.ProjectID = new SelectList(db.Projects, "ID", "NameVn", factoryWorkUnit.ProjectID);
            return View(factoryWorkUnit);
        }

        // GET: FactoryWorkUnit/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FactoryWorkUnit factoryWorkUnit = await db.FactoryWorkUnits.FindAsync(id);
            if (factoryWorkUnit == null)
            {
                return HttpNotFound();
            }
            ViewBag.FactoryDailyWorkID = new SelectList(db.FactoryDailyWorks, "ID", "NoteVn", factoryWorkUnit.FactoryDailyWorkID);
            ViewBag.ProjectID = new SelectList(db.Projects, "ID", "NameVn", factoryWorkUnit.ProjectID);
            return View(factoryWorkUnit);
        }

        // POST: FactoryWorkUnit/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,FactoryDailyWorkID,ProjectID,Type,EquipCount,NoteVn,NoteKr,Translate")] FactoryWorkUnit factoryWorkUnit)
        {
            if (ModelState.IsValid)
            {
                db.Entry(factoryWorkUnit).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.FactoryDailyWorkID = new SelectList(db.FactoryDailyWorks, "ID", "NoteVn", factoryWorkUnit.FactoryDailyWorkID);
            ViewBag.ProjectID = new SelectList(db.Projects, "ID", "NameVn", factoryWorkUnit.ProjectID);
            return View(factoryWorkUnit);
        }

        // GET: FactoryWorkUnit/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FactoryWorkUnit factoryWorkUnit = await db.FactoryWorkUnits.FindAsync(id);
            if (factoryWorkUnit == null)
            {
                return HttpNotFound();
            }
            return View(factoryWorkUnit);
        }

        // POST: FactoryWorkUnit/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            FactoryWorkUnit factoryWorkUnit = await db.FactoryWorkUnits.FindAsync(id);
            db.FactoryWorkUnits.Remove(factoryWorkUnit);
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
