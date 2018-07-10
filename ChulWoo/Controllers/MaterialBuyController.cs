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

namespace ChulWoo.Controllers
{
    public class MaterialBuyController : Controller
    {
        private ChulWooContext db = new ChulWooContext();

        // GET: MaterialBuy
        public async Task<ActionResult> Index()
        {
            var materialBuys = db.MaterialBuys.Include(m => m.Company).Include(m => m.Project);
            return View(await materialBuys.ToListAsync());
        }

        // GET: MaterialBuy/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MaterialBuy materialBuy = await db.MaterialBuys.FindAsync(id);
            if (materialBuy == null)
            {
                return HttpNotFound();
            }
            int eID = Convert.ToInt32(Session["LoginUserEmployeeID"]);
            Employee employee = db.Employees.FirstOrDefault(e => e.ID == eID);
            ViewBag.eName = employee.Name;
            ViewBag.eDepartment = employee.DepartmentVn;
            return View(materialBuy);
        }

        public async Task<ActionResult> EditAddUnit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MaterialBuy materialBuy = await db.MaterialBuys.FindAsync(id);
            if (materialBuy == null)
            {
                return HttpNotFound();
            }
            var materialBuyData = new MaterialBuyData();
            materialBuyData.MaterialBuy = materialBuy;

            return View(materialBuyData);
        }

        // POST: MaterialBuy/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAddUnit(MaterialBuyData materialBuyData)
        {
            if (ModelState.IsValid)
            {
                //                db.Entry(materialBuyData).State = EntityState.Modified;
                MaterialName mName = db.MaterialNames.FirstOrDefault(m => m.Name.Equals(materialBuyData.MaterialBuyUnit.MaterialUnitPrice.MaterialName.Name));
                if(mName != null)
                {
                    materialBuyData.MaterialBuyUnit.MaterialUnitPrice.MaterialNameID = mName.ID;
                    materialBuyData.MaterialBuyUnit.MaterialUnitPrice.MaterialName = mName;
                }
                else
                {
                    db.MaterialNames.Add(materialBuyData.MaterialBuyUnit.MaterialUnitPrice.MaterialName);
                }
                MaterialBuy materialBuy = db.MaterialBuys.FirstOrDefault(m => m.ID == materialBuyData.MaterialBuy.ID);
                materialBuyData.MaterialBuyUnit.MaterialUnitPrice.CompanyID = materialBuy.CompanyID;
                materialBuyData.MaterialBuyUnit.MaterialUnitPrice.Date = materialBuy.Date;

                MaterialUnitPrice muPrice = db.MaterialUnitPrices.OrderBy(m => m.Date)
                    .FirstOrDefault(m => m.MaterialName.Name.Equals(materialBuyData.MaterialBuyUnit.MaterialUnitPrice.MaterialName.Name));

                if (muPrice == null || muPrice.Price != materialBuyData.MaterialBuyUnit.MaterialUnitPrice.Price)
                {
                    db.MaterialUnitPrices.Add(materialBuyData.MaterialBuyUnit.MaterialUnitPrice);
                }
                else
                {
                    materialBuyData.MaterialBuyUnit.MaterialUnitPrice = muPrice;
                    materialBuyData.MaterialBuyUnit.MaterialUnitPriceID = muPrice.ID;
                }

                db.MaterialBuyUnits.Add(materialBuyData.MaterialBuyUnit);
                materialBuy.MaterialBuyUnits.Add(materialBuyData.MaterialBuyUnit);

                await db.SaveChangesAsync();
                return RedirectToAction("EditAddUnit", new { id = materialBuyData.MaterialBuy.ID });
            }
            return View(materialBuyData);
        }

        // GET: MaterialBuy/Create
        public ActionResult Create()
        {
            ViewBag.CompanyID = new SelectList(db.Companys, "ID", "Name");
            ViewBag.ProjectID = new SelectList(db.Projects, "ID", "Name");
            return View();
        }

        // POST: MaterialBuy/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,CompanyID,ProjectID,Date")] MaterialBuy materialBuy)
        {
            if (ModelState.IsValid)
            {
                db.MaterialBuys.Add(materialBuy);
                await db.SaveChangesAsync();

                return RedirectToAction("Edit", new { id = materialBuy.ID });
            }

            ViewBag.CompanyID = new SelectList(db.Companys, "ID", "Name", materialBuy.CompanyID);
            ViewBag.ProjectID = new SelectList(db.Projects, "ID", "Name", materialBuy.ProjectID);
            return View(materialBuy);
        }

        // GET: MaterialBuy/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MaterialBuy materialBuy = await db.MaterialBuys.FindAsync(id);
            if (materialBuy == null)
            {
                return HttpNotFound();
            }

            ViewBag.CompanyID = new SelectList(db.Companys, "ID", "Name", materialBuy.CompanyID);
            ViewBag.ProjectID = new SelectList(db.Projects, "ID", "Name", materialBuy.ProjectID);
            return View(materialBuy);
        }

        // POST: MaterialBuy/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,CompanyID,ProjectID,Date")] MaterialBuy materialBuy)
        {
            if (ModelState.IsValid)
            {
                db.Entry(materialBuy).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.CompanyID = new SelectList(db.Companys, "ID", "Name", materialBuy.CompanyID);
            ViewBag.ProjectID = new SelectList(db.Projects, "ID", "Name", materialBuy.ProjectID);
            return View(materialBuy);
        }

        [HttpPost]
        public JsonResult Edit2(string Prefix)
        {
            var names = db.MaterialNames.Where(m => m.Name.ToLower().StartsWith(Prefix.ToLower())).Select(m => new { m.Name }).ToList();
            return Json(names, JsonRequestBehavior.AllowGet);
        }

        // GET: MaterialBuy/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MaterialBuy materialBuy = await db.MaterialBuys.FindAsync(id);
            if (materialBuy == null)
            {
                return HttpNotFound();
            }
            return View(materialBuy);
        }

        // POST: MaterialBuy/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            MaterialBuy materialBuy = await db.MaterialBuys.FindAsync(id);
            db.MaterialBuys.Remove(materialBuy);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> DeleteUnit(int id, int unitid)
        {
            MaterialBuyUnit materialBuyUnit = await db.MaterialBuyUnits.FindAsync(unitid);
            int unitcount = db.MaterialBuyUnits.Count(m => m.MaterialUnitPriceID == materialBuyUnit.MaterialUnitPriceID);
            if( unitcount <= 1 )
            {
                int namecount = db.MaterialUnitPrices.Count(m => m.CompanyID == materialBuyUnit.MaterialUnitPrice.MaterialNameID);
                if (namecount <= 1)
                    db.MaterialNames.Remove(materialBuyUnit.MaterialUnitPrice.MaterialName);

                db.MaterialUnitPrices.Remove(materialBuyUnit.MaterialUnitPrice);
            }

            db.MaterialBuyUnits.Remove(materialBuyUnit);
            await db.SaveChangesAsync();
            return RedirectToAction("EditAddUnit", new { id = id });
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
