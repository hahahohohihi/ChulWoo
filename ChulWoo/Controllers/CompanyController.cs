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
    public class CompanyController : BaseController
    {
        private ChulWooContext db = new ChulWooContext();

        // GET: Company
        public async Task<ActionResult> Index(int? page, string currentFilter, string searchString)
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

            if (!String.IsNullOrEmpty(searchString))
            {
                var companys = db.Companys.Include(c => c.Projects.Select(p => p.Company))
                    .Include(c => c.MaterialBuys.Select(mb => mb.Project))
                    .Where(c => c.Name.Contains(searchString))
                    .OrderBy(c => c.Name);
                return View(companys.ToPagedList(pageNumber, pageSize));
            }
            else
            {
                var companys = db.Companys.Include(c => c.Projects.Select(p => p.Company))
                .Include(c => c.MaterialBuys.Select(mb => mb.Project))
                .OrderBy(c => c.Name);
                return View(companys.ToPagedList(pageNumber, pageSize));
            }

            //            return View(await db.Companys.ToListAsync());
        }

        // GET: Company/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (Session["LoginUserID"] == null)
                return RedirectToAction("Login", "Account");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

//            Company company = await db.Companys.FindAsync(id);
            var company = db.Companys.Include(c => c.Projects.Select(p => p.Company))
                .Include(c => c.MaterialBuys.Select(mb => mb.Project))
                .Where(c => c.ID == id)
                .Single();
            company.Projects = company.Projects.OrderByDescending(m => m.Date).ToList();
            company.MaterialBuys = company.MaterialBuys.OrderByDescending(m => m.Date).ToList();

            if (company == null)
            {
                return HttpNotFound();
            }
            return View(company);
        }

        // GET: Company/Create
        public ActionResult Create()
        {
            if (Session["LoginUserID"] == null)
                return RedirectToAction("Login", "Account");

            return View();
        }

        // POST: Company/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,Name,Address,Tel,Texcode,BankAccount,BankLocation")] Company company)
        {
            if (Session["LoginUserID"] == null)
                return RedirectToAction("Login", "Account");

            if (ModelState.IsValid)
            {
                db.Companys.Add(company);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(company);
        }

        // GET: Company/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (Session["LoginUserID"] == null)
                return RedirectToAction("Login", "Account");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Company company = await db.Companys.FindAsync(id);
            if (company == null)
            {
                return HttpNotFound();
            }
            return View(company);
        }

        // POST: Company/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,Name,Address,Tel,Texcode,BankAccount,BankLocation")] Company company)
        {
            if (Session["LoginUserID"] == null)
                return RedirectToAction("Login", "Account");

            if (ModelState.IsValid)
            {
                db.Entry(company).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(company);
        }

        // GET: Company/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (Session["LoginUserID"] == null)
                return RedirectToAction("Login", "Account");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Company company = await db.Companys.FindAsync(id);
            if (company == null)
            {
                return HttpNotFound();
            }
            return View(company);
        }

        // POST: Company/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            if (Session["LoginUserID"] == null)
                return RedirectToAction("Login", "Account");

            Company company = await db.Companys.FindAsync(id);
            db.Companys.Remove(company);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> AddPayment(int? id)
        {
            if (Session["LoginUserID"] == null)
                return RedirectToAction("Login", "Account");

            if (Convert.ToInt32(Session["LoginUserSecurity"]) < Convert.ToInt32(Security.Personnel))
                return RedirectToAction("Index", "Home");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MaterialBuy materialBuy = await db.MaterialBuys.FindAsync(id);
            if (materialBuy == null)
            {
                return HttpNotFound();
            }

            var paymentsum = materialBuy.Payments.Sum(p => p.Amount);
            var price = materialBuy.MaterialBuyUnits.Sum(m => m.Quantity * m.MaterialUnitPrice.Price);
            var amount = price + price * materialBuy.VATPer - paymentsum;
            if (amount > 0)
            {
                Payment payment = new Payment();
                payment.CompanyID = materialBuy.CompanyID;
                payment.Date = DateTime.Now;
                payment.EmployeeID = Convert.ToInt32(Session["LoginUserEmployeeID"]);
                payment.ProjectID = materialBuy.ProjectID;
                payment.StatementType = Models.StatementType.Payment;
                payment.Type = PaymentType.Bank;
                payment.Amount = (double)amount;

                db.Payments.Add(payment);
                materialBuy.Payments.Add(payment);

                await db.SaveChangesAsync();
            }

            return RedirectToAction("Details", "Company", new { id = materialBuy.CompanyID });
        }


        public async Task<ActionResult> AllPaymentIndex( int id, DateTime? startDate, DateTime? endDate )
        {
            if (Session["LoginUserID"] == null)
                return RedirectToAction("Login", "Account");

//            var materialBuys = db.MaterialBuys.Where( m => m.CompanyID == id )
//                .Include(m => m.Company).Include(m => m.Project)
//                .OrderByDescending(m => m.ID).OrderByDescending(m => m.Date);

            var company = db.Companys.Include(c => c.Projects.Select(p => p.Company))
                .Include(c => c.MaterialBuys.Select(mb => mb.Project))
                .Where(c => c.ID == id)
                .Single();
            company.Projects = company.Projects.OrderByDescending(m => m.Date).ToList();
            if( startDate != null && endDate != null)
            {
                company.MaterialBuys = company.MaterialBuys.Where(m=> m.Date <= endDate && m.Date >= startDate)
                    .OrderByDescending(m => m.Date).ToList();
            }
            else
                company.MaterialBuys = company.MaterialBuys.OrderByDescending(m => m.Date).ToList();

            AllPaymentIndexData allPayment = new AllPaymentIndexData();
            allPayment.Company = company;
            allPayment.StartDate = startDate;
            allPayment.EndDate = endDate;

            return View(allPayment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AllPaymentIndex([Bind(Include = "Company, StartDate, EndDate, Note, PaymentDate")] AllPaymentIndexData allPayment)
        {
            if (Session["LoginUserID"] == null)
                return RedirectToAction("Login", "Account");

            allPayment.Company = db.Companys.FirstOrDefault(c => c.ID == allPayment.Company.ID);
            if (allPayment.Company == null)
            {
                return HttpNotFound();
            }

            IEnumerable<MaterialBuy> MaterialBuys = db.MaterialBuys.Where(m => m.CompanyID == allPayment.Company.ID && m.Date <= allPayment.EndDate && m.Date >= allPayment.StartDate)
                    .OrderByDescending(m => m.Date).ToList();

            if( allPayment.PaymentDate == null )
                allPayment.PaymentDate = DateTime.Now;

            Payment payment = new Payment();
            payment.CompanyID = allPayment.Company.ID;
            payment.Date = allPayment.PaymentDate;
            payment.EmployeeID = Convert.ToInt32(Session["LoginUserEmployeeID"]);
//            payment.ProjectID = ;
            payment.StatementType = Models.StatementType.Payment;
            payment.Type = PaymentType.Bank;
            payment.Amount = 0;
            payment.NoteVn = allPayment.Note;
            payment.NoteKr = allPayment.Note;
            payment.MaterialBuys = new List<MaterialBuy>();

            foreach ( var item in MaterialBuys)
            {
                var paymentsum = item.Payments.Sum(p => p.Amount);
                var price = item.MaterialBuyUnits.Sum(m => m.Quantity * m.MaterialUnitPrice.Price);
                var amount = price + price * item.VATPer - paymentsum;
                if (amount > 0)
                {
                    payment.Amount += (double)amount;
                    payment.MaterialBuys.Add(item);

                    item.Payments.Add(payment);
                }
            }
            if( payment.Amount > 0 )
                db.Payments.Add(payment);

            await db.SaveChangesAsync();

            allPayment.Company.MaterialBuys = allPayment.Company.MaterialBuys.Where(m => m.CompanyID == allPayment.Company.ID && m.Date <= allPayment.EndDate && m.Date >= allPayment.StartDate)
                    .OrderByDescending(m => m.Date).ToList();

            return View(allPayment);
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        [HttpPost]
        public JsonResult SetNameList(string Prefix)
        {
            var names = db.Companys.Where(c => c.Name.ToLower().Contains(Prefix.ToLower())).Select(c => new { c.Name }).ToList();
            return Json(names, JsonRequestBehavior.AllowGet);
        }
    }
}
