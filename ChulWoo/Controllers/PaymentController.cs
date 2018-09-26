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
    public class PaymentController : BaseController
    {
        private ChulWooContext db = new ChulWooContext();

        // GET: Payment
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

            var payments = db.Payments.Include(p => p.Company)
                .Include(p => p.Employee)
//                .Include(p => p.MaterialBuy)
                .OrderByDescending(p => p.Date);

            if (!String.IsNullOrEmpty(searchString))
                payments = (IOrderedQueryable<Payment>)payments.Where(p => p.Company.Name.Contains(searchString));

            if (translate == true)
                payments = (IOrderedQueryable<Payment>)payments.Where(p => !p.Translate);

            return View(payments.ToPagedList(pageNumber, pageSize));
//            return View(await payments.ToListAsync());
        }

        // GET: Payment/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Payment payment = await db.Payments.FindAsync(id);
            if (payment == null)
            {
                return HttpNotFound();
            }
            return View(payment);
        }

        // GET: Payment/Create
        public ActionResult Create()
        {
            ViewBag.CompanyID = new SelectList(db.Companys, "ID", "Name");
            ViewBag.EmployeeID = new SelectList(db.Employees, "ID", "DepartmentVn");
            ViewBag.MaterialBuyID = new SelectList(db.MaterialBuys, "ID", "NoteVn");
            return View();
        }

        // POST: Payment/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,EmployeeID,MaterialBuyID,CompanyID,Date,Amount,AmountString,Type,StatementType,NoteVn,NoteKr,Translate")] Payment payment)
        {
            if (ModelState.IsValid)
            {
                db.Payments.Add(payment);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.CompanyID = new SelectList(db.Companys, "ID", "Name", payment.CompanyID);
            ViewBag.EmployeeID = new SelectList(db.Employees, "ID", "DepartmentVn", payment.EmployeeID);
//            ViewBag.MaterialBuyID = new SelectList(db.MaterialBuys, "ID", "NoteVn", payment.MaterialBuyID);
            return View(payment);
        }

        // GET: Payment/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Payment payment = await db.Payments.FindAsync(id);
            if (payment == null)
            {
                return HttpNotFound();
            }
            ViewBag.CompanyID = new SelectList(db.Companys, "ID", "Name", payment.CompanyID);
            ViewBag.EmployeeID = new SelectList(db.Employees, "ID", "DepartmentVn", payment.EmployeeID);
//            ViewBag.MaterialBuyID = new SelectList(db.MaterialBuys, "ID", "NoteVn", payment.MaterialBuyID);
            return View(payment);
        }

        // POST: Payment/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,EmployeeID,MaterialBuyID,CompanyID,ProjectID,Date,Amount,AmountString,Type,StatementType,NoteVn,NoteKr,Translate")] Payment payment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(payment).State = EntityState.Modified;
                await db.SaveChangesAsync();
//                return RedirectToAction("Index");
                return RedirectToAction("Index", new { translate = Session["Translate"] });
            }
            ViewBag.CompanyID = new SelectList(db.Companys, "ID", "Name", payment.CompanyID);
            ViewBag.EmployeeID = new SelectList(db.Employees, "ID", "DepartmentVn", payment.EmployeeID);
//            ViewBag.MaterialBuyID = new SelectList(db.MaterialBuys, "ID", "NoteVn", payment.MaterialBuyID);
            return View(payment);
        }

        // GET: Payment/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Payment payment = await db.Payments.FindAsync(id);
            if (payment == null)
            {
                return HttpNotFound();
            }
            return View(payment);
        }

        // POST: Payment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Payment payment = await db.Payments.FindAsync(id);
            db.Payments.Remove(payment);
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
