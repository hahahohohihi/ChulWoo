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
    public class FactoryDailyWorkController : BaseController
    {
        private ChulWooContext db = new ChulWooContext();

        // GET: FactoryDailyWork
        public async Task<ActionResult> Index()
        {
            var factoryDailyWorks = db.FactoryDailyWorks.Include(f => f.ProparingPerson);
            return View(await factoryDailyWorks.ToListAsync());
        }

        // GET: FactoryDailyWork/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FactoryDailyWork factoryDailyWork = await db.FactoryDailyWorks.FindAsync(id);
            if (factoryDailyWork == null)
            {
                return HttpNotFound();
            }
            return View(factoryDailyWork);
        }

        // GET: FactoryDailyWork/Create
        public ActionResult Create()
        {
            var employees = db.Employees.Where(e => e.ResignID == null);
            ViewBag.ProparingPersonID = new SelectList(employees, "ID", "Name", Session["LoginUserEmployeeID"]);
            return View();
        }

        // POST: FactoryDailyWork/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,ProparingPersonID,Date,NoteVn,NoteKr,Translate")] FactoryDailyWork factoryDailyWork)
        {
            if (ModelState.IsValid)
            {
                db.FactoryDailyWorks.Add(factoryDailyWork);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            var employees = db.Employees.Where(e => e.ResignID == null);
            ViewBag.ProparingPersonID = new SelectList(employees, "ID", "Name", factoryDailyWork.ProparingPersonID);
            return View(factoryDailyWork);
        }

        // GET: FactoryDailyWork/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FactoryDailyWork factoryDailyWork = await db.FactoryDailyWorks.FindAsync(id);
            if (factoryDailyWork == null)
            {
                return HttpNotFound();
            }
            var employees = db.Employees.Where(e => e.ResignID == null);
            ViewBag.ProparingPersonID = new SelectList(employees, "ID", "Name", factoryDailyWork.ProparingPersonID);
            return View(factoryDailyWork);
        }

        // POST: FactoryDailyWork/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,ProparingPersonID,Date,NoteVn,NoteKr,Translate")] FactoryDailyWork factoryDailyWork)
        {
            if (ModelState.IsValid)
            {
                db.Entry(factoryDailyWork).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            var employees = db.Employees.Where(e => e.ResignID == null);
            ViewBag.ProparingPersonID = new SelectList(employees, "ID", "Name", factoryDailyWork.ProparingPersonID);
            return View(factoryDailyWork);
        }

        // GET: FactoryDailyWork/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FactoryDailyWork factoryDailyWork = await db.FactoryDailyWorks.FindAsync(id);
            if (factoryDailyWork == null)
            {
                return HttpNotFound();
            }
            return View(factoryDailyWork);
        }

        // POST: FactoryDailyWork/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            FactoryDailyWork factoryDailyWork = await db.FactoryDailyWorks.FindAsync(id);
            db.FactoryDailyWorks.Remove(factoryDailyWork);
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
