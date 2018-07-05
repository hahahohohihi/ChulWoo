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
            return View(materialBuy);
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
                return RedirectToAction("Index");
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
            var materialBuyData = new MaterialBuyData();
            materialBuyData.MaterialBuy = materialBuy;

            ViewBag.CompanyID = new SelectList(db.Companys, "ID", "Name", materialBuy.CompanyID);
            ViewBag.ProjectID = new SelectList(db.Projects, "ID", "Name", materialBuy.ProjectID);
            return View(materialBuyData);
        }

        // POST: MaterialBuy/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,CompanyID,ProjectID,Date")] MaterialBuyData materialBuyData)
        {
            if (ModelState.IsValid)
            {
                db.Entry(materialBuyData).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.CompanyID = new SelectList(db.Companys, "ID", "Name", materialBuyData.MaterialBuy.CompanyID);
            ViewBag.ProjectID = new SelectList(db.Projects, "ID", "Name", materialBuyData.MaterialBuy.ProjectID);
            return View(materialBuyData);
        }

        [HttpPost]
        public JsonResult Edit(string Prefix)
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
