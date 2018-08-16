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
    public class PersonnelController : BaseController
    {
        private ChulWooContext db = new ChulWooContext();

        // GET: Personnel
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
            ViewBag.translate = translate;

            var personnels = db.Personnels.Include(p => p.Employee).OrderByDescending(p => p.SendDate);

            if (!String.IsNullOrEmpty(searchString))
                personnels = (IOrderedQueryable<Personnel>)personnels.Where(p => p.Employee.Name.Contains(searchString));

            if (translate == true)
                personnels = (IOrderedQueryable<Personnel>)personnels.Where(p => !p.Translate);

            return View(personnels.ToPagedList(pageNumber, pageSize));
        }

        // GET: Personnel/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (Session["LoginUserID"] == null)
                return RedirectToAction("Login", "Account");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Personnel personnel = await db.Personnels.FindAsync(id);
            if (personnel == null)
            {
                return HttpNotFound();
            }

            ViewBag.TotalDays = Convert.ToInt32(((TimeSpan)(personnel.EndDate - personnel.StartDate)).TotalDays) + 1;

            int year = personnel.SendDate.Value.Year;
            DateTime temp = new DateTime(year, 1, 1);
            int holydayCount = GetDiffMonths(temp, (DateTime)personnel.EndDate);

            List<Personnel> personnels = db.Personnels.Where(p => p.EmployeeID == personnel.EmployeeID &&
                                                                p.ID != id &&
                                                                p.StartDate.Value.Year == year &&
                                                                p.StartDate < personnel.StartDate &&
                                                                p.Type == PersonnelType.AnnualLeave).ToList();
            int count = 0;
            foreach( Personnel item in personnels )
            {
                count += Convert.ToInt32(((TimeSpan)(item.EndDate - item.StartDate)).TotalDays) + 1;
            }

            ViewBag.TotalHolydays = holydayCount - count;


            return View(personnel);
        }

        public int GetDiffMonths(DateTime from, DateTime to)
        {
            int diff = 0;
            DateTime added = from;

            while (true)
            {
                added = added.AddMonths(1);

                if (added > to)
                {
                    return diff;
                }

                diff++;
            }
        }

        // GET: Personnel/Create
        public ActionResult Create()
        {
            if (Session["LoginUserID"] == null)
                return RedirectToAction("Login", "Account");

            ViewBag.EmployeeID = new SelectList(db.Employees, "ID", "Name");
            return View();
        }

        // POST: Personnel/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,EmployeeID,SendDate,ReasonVn,ReasonKr,StartDate,EndDate,Type,Translate")] Personnel personnel)
        {
            if (Session["LoginUserID"] == null)
                return RedirectToAction("Login", "Account");

            if (ModelState.IsValid)
            {
                personnel.SendDate = DateTime.Now;
                db.Personnels.Add(personnel);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.EmployeeID = new SelectList(db.Employees, "ID", "Name", personnel.EmployeeID);
            return View(personnel);
        }

        // GET: Personnel/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (Session["LoginUserID"] == null)
                return RedirectToAction("Login", "Account");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Personnel personnel = await db.Personnels.FindAsync(id);
            if (personnel == null)
            {
                return HttpNotFound();
            }
            ViewBag.EmployeeID = new SelectList(db.Employees, "ID", "Name", personnel.EmployeeID);
            return View(personnel);
        }

        // POST: Personnel/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,EmployeeID,SendDate,ReasonVn,ReasonKr,StartDate,EndDate,Type,Translate")] Personnel personnel)
        {
            if (Session["LoginUserID"] == null)
                return RedirectToAction("Login", "Account");

            if (ModelState.IsValid)
            {
                db.Entry(personnel).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.EmployeeID = new SelectList(db.Employees, "ID", "Name", personnel.EmployeeID);
            return View(personnel);
        }

        // GET: Personnel/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (Session["LoginUserID"] == null)
                return RedirectToAction("Login", "Account");

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
            if (Session["LoginUserID"] == null)
                return RedirectToAction("Login", "Account");

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
