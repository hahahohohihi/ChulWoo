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
using PagedList;
using ChulWoo.Viewmodel;

namespace ChulWoo.Controllers
{
    public class FactoryDailyWorkController : BaseController
    {

        private ChulWooContext db = new ChulWooContext();

        // GET: FactoryDailyWork
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

            var factoryDailyWorks = db.FactoryDailyWorks.Include(f => f.ProparingPerson).OrderByDescending( f => f.Date );
/*
            foreach( var work in factoryDailyWorks )
            {
                work.FactoryWorkUnits = db.FactoryWorkUnits.Where(f => f.FactoryDailyWorkID == work.ID).ToList();

                foreach( var unit in work.FactoryWorkUnits)
                {
                    unit.Project = db.Projects.FirstOrDefault(p => p)
                }
            }
*/
            if (!String.IsNullOrEmpty(searchString))
                factoryDailyWorks = (IOrderedQueryable<FactoryDailyWork>)factoryDailyWorks.Where(f => f.FactoryWorkUnits.Count(fu => fu.Project.NameKr.Contains(searchString) || fu.Project.NameVn.Contains(searchString)) > 0);

            if (translate == true)
                factoryDailyWorks = (IOrderedQueryable<FactoryDailyWork>)factoryDailyWorks.Where(f => !f.Translate);

            //            var projects = db.Projects.Include(p => p.Company);
            return View(factoryDailyWorks.ToPagedList(pageNumber, pageSize));
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
//                return RedirectToAction("Index");
                return RedirectToAction("EditAddWorkUnit", new { id = factoryDailyWork.ID });
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

        public async Task<ActionResult> EditAddWorkUnit(int? id)
        {
            if (Session["LoginUserID"] == null)
                return RedirectToAction("Login", "Account");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FactoryDailyWork dailyWork = await db.FactoryDailyWorks.FindAsync(id);
            if (dailyWork == null)
            {
                return HttpNotFound();
            }
            var factoryDailyWorkUnitData = new FactoryDailyWorkUnitData();
            factoryDailyWorkUnitData.DailyWork = dailyWork;

//            ViewBag.WorkUnits = db.FactoryWorkUnits.Where(w => w.StartDate <= dailyWork.Date && (w.Complete == false || w.EndDate >= dailyWork.Date)).ToList();

            if (Convert.ToInt32(Session["CurrentCulture"]) == 2)
                ViewBag.ProjectID = new SelectList(db.Projects.OrderByDescending(p => p.Date), "ID", "NameKr");
            else
                ViewBag.ProjectID = new SelectList(db.Projects.OrderByDescending(p => p.Date), "ID", "NameVn");

            return View(factoryDailyWorkUnitData);
        }

        // POST: MaterialBuy/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAddWorkUnit(FactoryDailyWorkUnitData factoryDailyWorkUnitData)
        {
            if (Session["LoginUserID"] == null)
                return RedirectToAction("Login", "Account");

            if (ModelState.IsValid)
            {
                //                db.Entry(materialBuyData).State = EntityState.Modified;
                FactoryDailyWork dailyWork = db.FactoryDailyWorks.FirstOrDefault(m => m.ID == factoryDailyWorkUnitData.DailyWork.ID);

                factoryDailyWorkUnitData.WorkUnit.FactoryDailyWorkID = factoryDailyWorkUnitData.DailyWork.ID;
//                factoryDailyWorkUnitData.WorkUnit.Date = (DateTime)factoryDailyWorkUnitData.DailyWork..Date;

                FactoryWorkUnit workTemp = db.FactoryWorkUnits.FirstOrDefault(e => e.NoteVn.Equals(factoryDailyWorkUnitData.WorkUnit.NoteVn));
                if (workTemp != null)
                {
                    factoryDailyWorkUnitData.WorkUnit.NoteKr = workTemp.NoteKr;
                }
                else
                    factoryDailyWorkUnitData.WorkUnit.NoteKr = factoryDailyWorkUnitData.WorkUnit.NoteVn;


                db.FactoryWorkUnits.Add(factoryDailyWorkUnitData.WorkUnit);
                dailyWork.FactoryWorkUnits.Add(factoryDailyWorkUnitData.WorkUnit);

                await db.SaveChangesAsync();
                return RedirectToAction("EditAddWorkUnit", new { id = factoryDailyWorkUnitData.DailyWork.ID });
            }
            return View(factoryDailyWorkUnitData);
        }

        public async Task<ActionResult> EditEditWorkUnit(int? id, int workunitid)
        {
            if (Session["LoginUserID"] == null)
                return RedirectToAction("Login", "Account");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FactoryDailyWork dailyWork = await db.FactoryDailyWorks.FindAsync(id);
            if (dailyWork == null)
            {
                return HttpNotFound();
            }
            FactoryWorkUnit workUnit = await db.FactoryWorkUnits.FindAsync(workunitid);
            if (workUnit == null)
            {
                return HttpNotFound();
            }

            if (Convert.ToInt32(Session["CurrentCulture"]) == 2)
            {
                workUnit.NoteVn = workUnit.NoteKr;
            }
            var factoryDailyWorkUnitData = new FactoryDailyWorkUnitData();
            factoryDailyWorkUnitData.DailyWork = dailyWork;
            factoryDailyWorkUnitData.WorkUnit = workUnit;

            factoryDailyWorkUnitData.DailyWork.FactoryWorkUnits = factoryDailyWorkUnitData.DailyWork.FactoryWorkUnits.OrderBy(p => p.ID).ToList();

//            ViewBag.WorkUnits = db.WorkUnits.Where(w => w.StartDate <= dailyWork.Date && (w.Complete == false || w.EndDate >= dailyWork.Date)).ToList();

            if (Convert.ToInt32(Session["CurrentCulture"]) == 2)
                ViewBag.ProjectID = new SelectList(db.Projects.OrderByDescending(p => p.Date), "ID", "NameKr");
            else
                ViewBag.ProjectID = new SelectList(db.Projects.OrderByDescending(p => p.Date), "ID", "NameVn");

            return View(factoryDailyWorkUnitData);
        }

        // POST: MaterialBuy/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditEditWorkUnit(int id, int workunitid, FactoryDailyWorkUnitData factoryDailyWorkUnitData)
        {
            if (Session["LoginUserID"] == null)
                return RedirectToAction("Login", "Account");

            if (ModelState.IsValid)
            {
                //                db.Entry(materialBuyData).State = EntityState.Modified;
                DailyWork dailyWork = db.DailyWorks.FirstOrDefault(m => m.ID == factoryDailyWorkUnitData.DailyWork.ID);
                FactoryWorkUnit workUnit = await db.FactoryWorkUnits.FindAsync(workunitid);

//                workUnit.Date = (DateTime)factoryDailyWorkUnitData.DailyWork.Date;
                workUnit.FactoryDailyWorkID = factoryDailyWorkUnitData.DailyWork.ID;
                workUnit.ProjectID = factoryDailyWorkUnitData.WorkUnit.ProjectID;
//                equipmentUnit.NameKr = factoryDailyWorkUnitData.WorkUnit.NameKr;
//                equipmentUnit.NameVn = factoryDailyWorkUnitData.WorkUnit.NameVn;

                if (Convert.ToInt32(Session["CurrentCulture"]) == 2)
                {
                    if (!workUnit.NoteKr.Equals(factoryDailyWorkUnitData.WorkUnit.NoteVn))
                    {
                        workUnit.NoteVn = factoryDailyWorkUnitData.WorkUnit.NoteVn;
                        workUnit.NoteKr = factoryDailyWorkUnitData.WorkUnit.NoteVn;
                        workUnit.Translate = false;
                    }
                }
                else
                {
                    if (!workUnit.NoteVn.Equals(factoryDailyWorkUnitData.WorkUnit.NoteVn))
                    {
                        workUnit.NoteVn = factoryDailyWorkUnitData.WorkUnit.NoteVn;
                        workUnit.NoteKr = factoryDailyWorkUnitData.WorkUnit.NoteVn;
                        workUnit.Translate = false;
                    }
                }
                workUnit.EquipCount = factoryDailyWorkUnitData.WorkUnit.EquipCount;

                await db.SaveChangesAsync();
                return RedirectToAction("EditAddWorkUnit", new { id = factoryDailyWorkUnitData.DailyWork.ID });
            }
            return View(factoryDailyWorkUnitData);
        }

        public async Task<ActionResult> DeleteWorkUnit(int id, int workunitid)
        {
            FactoryWorkUnit workUnit = await db.FactoryWorkUnits.FindAsync(workunitid);

            db.FactoryWorkUnits.Remove(workUnit);

            //PreDeleteUnit(id, paymentid);
            await db.SaveChangesAsync();
            return RedirectToAction("Details", new { id = id });
        }

        [HttpPost]
        public JsonResult SelectWorkUnit(string Prefix)
        {
            var names = db.FactoryWorkUnits.Where(m => m.NoteVn.ToLower().Contains(Prefix.ToLower())).Select(m => new { m.NoteVn }).Distinct().ToList();
            return Json(names, JsonRequestBehavior.AllowGet);
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
