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
    public class PersonnelController : Controller
    {
        private ChulWooContext db = new ChulWooContext();

        // GET: Personnel
        public async Task<ActionResult> Index()
        {
            var personnels = db.Personnels.Include(p => p.Employee);
            return View(await personnels.ToListAsync());
        }

        // GET: Personnel/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Personnel personnel = await db.Personnels.FindAsync(id);
            if (personnel == null)
            {
                return HttpNotFound();
            }
            return View(personnel);
        }

        // GET: Personnel/Create
        public ActionResult Create()
        {
            ViewBag.EmployeeID = new SelectList(db.Employees, "ID", "DepartmentVn");
            return View();
        }

        // POST: Personnel/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,EmployeeID,StartDate,EndDate,Type")] Personnel personnel)
        {
            if (ModelState.IsValid)
            {
                db.Personnels.Add(personnel);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.EmployeeID = new SelectList(db.Employees, "ID", "DepartmentVn", personnel.EmployeeID);
            return View(personnel);
        }

        // GET: Personnel/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Personnel personnel = await db.Personnels.FindAsync(id);
            if (personnel == null)
            {
                return HttpNotFound();
            }
            ViewBag.EmployeeID = new SelectList(db.Employees, "ID", "DepartmentVn", personnel.EmployeeID);
            return View(personnel);
        }

        // POST: Personnel/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,EmployeeID,StartDate,EndDate,Type")] Personnel personnel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(personnel).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.EmployeeID = new SelectList(db.Employees, "ID", "DepartmentVn", personnel.EmployeeID);
            return View(personnel);
        }

        // GET: Personnel/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Personnel personnel = await db.Personnels.FindAsync(id);
            if (personnel == null)
            {
                return HttpNotFound();
            }
            return View(personnel);
        }

        // POST: Personnel/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Personnel personnel = await db.Personnels.FindAsync(id);
            db.Personnels.Remove(personnel);
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
