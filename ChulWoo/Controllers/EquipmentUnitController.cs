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
    public class EquipmentUnitController : BaseController
    {
        private ChulWooContext db = new ChulWooContext();

        // GET: EquipmentUnit
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

            var equipmentUnits = db.EquipmentUnits.Include(e => e.DailyWork).OrderByDescending(e => e.Date);

            if (!String.IsNullOrEmpty(searchString))
                equipmentUnits = (IOrderedQueryable<EquipmentUnit>)equipmentUnits.Where(p => p.NoteVn.Contains(searchString));

            if (translate == true)
                equipmentUnits = (IOrderedQueryable<EquipmentUnit>)equipmentUnits.Where(p => !p.Translate);

            return View(equipmentUnits.ToPagedList(pageNumber, pageSize));
        }

        // GET: EquipmentUnit/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EquipmentUnit equipmentUnit = await db.EquipmentUnits.FindAsync(id);
            if (equipmentUnit == null)
            {
                return HttpNotFound();
            }
            return View(equipmentUnit);
        }

        // GET: EquipmentUnit/Create
        public ActionResult Create()
        {
            ViewBag.DailyWorkID = new SelectList(db.DailyWorks, "ID", "WeatherVn");
            return View();
        }

        // POST: EquipmentUnit/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,DailyWorkID,Date,NameVn,NameKr,NoteVn,NoteKr,EquipCount,Translate")] EquipmentUnit equipmentUnit)
        {
            if (ModelState.IsValid)
            {
                db.EquipmentUnits.Add(equipmentUnit);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.DailyWorkID = new SelectList(db.DailyWorks, "ID", "WeatherVn", equipmentUnit.DailyWorkID);
            return View(equipmentUnit);
        }

        // GET: EquipmentUnit/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EquipmentUnit equipmentUnit = await db.EquipmentUnits.FindAsync(id);
            if (equipmentUnit == null)
            {
                return HttpNotFound();
            }
            ViewBag.DailyWorkID = new SelectList(db.DailyWorks, "ID", "WeatherVn", equipmentUnit.DailyWorkID);
            return View(equipmentUnit);
        }

        // POST: EquipmentUnit/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,DailyWorkID,Date,NameVn,NameKr,NoteVn,NoteKr,EquipCount,Translate")] EquipmentUnit equipmentUnit)
        {
            if (ModelState.IsValid)
            {
                db.Entry(equipmentUnit).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index", new { translate = Session["Translate"] });
            }
            ViewBag.DailyWorkID = new SelectList(db.DailyWorks, "ID", "WeatherVn", equipmentUnit.DailyWorkID);
            return View(equipmentUnit);
        }

        // GET: EquipmentUnit/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EquipmentUnit equipmentUnit = await db.EquipmentUnits.FindAsync(id);
            if (equipmentUnit == null)
            {
                return HttpNotFound();
            }
            return View(equipmentUnit);
        }

        // POST: EquipmentUnit/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            EquipmentUnit equipmentUnit = await db.EquipmentUnits.FindAsync(id);
            db.EquipmentUnits.Remove(equipmentUnit);
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
