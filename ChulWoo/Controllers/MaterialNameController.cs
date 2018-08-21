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

namespace ChulWoo.Controllers
{
    public class MaterialNameController : BaseController
    {
        private ChulWooContext db = new ChulWooContext();

        // GET: MaterialName
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

            var names = db.MaterialNames.OrderByDescending(m => m.ID);

            if (!String.IsNullOrEmpty(searchString))
                names = (IOrderedQueryable<MaterialName>)names.Where(p => p.NameVn.Contains(searchString) || p.NameKr.Contains(searchString));

            if (translate == true)
                names = (IOrderedQueryable<MaterialName>)names.Where(p => !p.Translate);

            return View(names.ToPagedList(pageNumber, pageSize));
//            return View(await db.MaterialNames.OrderByDescending(m => m.ID).ToListAsync());
        }

        // GET: MaterialName/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (Session["LoginUserID"] == null)
                return RedirectToAction("Login", "Account");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MaterialName materialName = await db.MaterialNames.FindAsync(id);
            if (materialName == null)
            {
                return HttpNotFound();
            }
            return View(materialName);
        }

        // GET: MaterialName/Create
        public ActionResult Create()
        {
            if (Session["LoginUserID"] == null)
                return RedirectToAction("Login", "Account");

            return View();
        }

        // POST: MaterialName/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,NameVn,NameKr,Sort")] MaterialName materialName)
        {
            if (Session["LoginUserID"] == null)
                return RedirectToAction("Login", "Account");

            if (ModelState.IsValid)
            {
                db.MaterialNames.Add(materialName);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(materialName);
        }

        // GET: MaterialName/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (Session["LoginUserID"] == null)
                return RedirectToAction("Login", "Account");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MaterialName materialName = await db.MaterialNames.FindAsync(id);
            if (materialName == null)
            {
                return HttpNotFound();
            }
            return View(materialName);
        }

        // POST: MaterialName/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,NameVn,NameKr,Sort,Translate")] MaterialName materialName)
        {
            if (Session["LoginUserID"] == null)
                return RedirectToAction("Login", "Account");

            if (ModelState.IsValid)
            {
                db.Entry(materialName).State = EntityState.Modified;
                await db.SaveChangesAsync();
//                return RedirectToAction("Index");
                return RedirectToAction("Index", new { translate = Session["Translate"] });
            }
            return View(materialName);
        }

        // GET: MaterialName/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (Session["LoginUserID"] == null)
                return RedirectToAction("Login", "Account");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MaterialName materialName = await db.MaterialNames.FindAsync(id);
            if (materialName == null)
            {
                return HttpNotFound();
            }
            return View(materialName);
        }

        // POST: MaterialName/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            if (Session["LoginUserID"] == null)
                return RedirectToAction("Login", "Account");

            MaterialName materialName = await db.MaterialNames.FindAsync(id);
            db.MaterialNames.Remove(materialName);
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
