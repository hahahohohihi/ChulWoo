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
using PagedList;

namespace ChulWoo.Controllers
{
    public class MaterialBuyController : BaseController
    {
        private ChulWooContext db = new ChulWooContext();

        // GET: MaterialBuy
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

            var materialBuys = db.MaterialBuys.Include(m => m.Company).Include(m => m.Project)
                .OrderByDescending(m => m.Date);

            if (!String.IsNullOrEmpty(searchString))
                materialBuys = (IOrderedQueryable<MaterialBuy>)materialBuys.Where(m => m.Company.Name.Contains(searchString) || 
                    m.Project.NameKr.Contains(searchString) || m.Project.NameVn.Contains(searchString) );

            if (translate == true)
                materialBuys = (IOrderedQueryable<MaterialBuy>)materialBuys.Where(m => !m.Translate);

            return View(materialBuys.ToPagedList(pageNumber, pageSize));
        }

        // GET: MaterialBuy/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (Session["LoginUserID"] == null)
                return RedirectToAction("Login", "Account");

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


        public async Task<ActionResult> EditAddPayment(int? id)
        {
            if (Session["LoginUserID"] == null)
                return RedirectToAction("Login", "Account");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MaterialBuy materialBuy = await db.MaterialBuys.FindAsync(id);
            if (materialBuy == null)
            {
                return HttpNotFound();
            }
            var materialBuyPayment = new MaterialBuyPaymentData();
            materialBuyPayment.MaterialBuy = materialBuy;

            return View(materialBuyPayment);
        }

        // POST: MaterialBuy/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAddPayment(MaterialBuyPaymentData materialBuyPaymentData)
        {
            if (Session["LoginUserID"] == null)
                return RedirectToAction("Login", "Account");

            if (ModelState.IsValid)
            {
                //                db.Entry(materialBuyData).State = EntityState.Modified;
                MaterialBuy materialBuy = db.MaterialBuys.FirstOrDefault(m => m.ID == materialBuyPaymentData.MaterialBuy.ID);

                materialBuyPaymentData.Payment.EmployeeID = Convert.ToInt32(Session["LoginUserEmployeeID"]);
                materialBuyPaymentData.Payment.Employee = db.Employees.FirstOrDefault(e => e.ID == materialBuyPaymentData.Payment.EmployeeID);

                db.Payments.Add(materialBuyPaymentData.Payment);
                materialBuy.Payments.Add(materialBuyPaymentData.Payment);

                await db.SaveChangesAsync();
                return RedirectToAction("EditAddPayment", new { id = materialBuyPaymentData.MaterialBuy.ID });
            }
            return View(materialBuyPaymentData);
        }
        public async Task<ActionResult> EditEditPayment(int? id, int paymentid)
        {
            if (Session["LoginUserID"] == null)
                return RedirectToAction("Login", "Account");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MaterialBuy materialBuy = await db.MaterialBuys.FindAsync(id);
            if (materialBuy == null)
            {
                return HttpNotFound();
            }
            Payment payment = await db.Payments.FindAsync(paymentid);
            if (payment == null)
            {
                return HttpNotFound();
            }
            var materialBuyPayment = new MaterialBuyPaymentData();
            materialBuyPayment.MaterialBuy = materialBuy;
            materialBuyPayment.Payment = payment;

            materialBuyPayment.MaterialBuy.Payments = materialBuyPayment.MaterialBuy.Payments.OrderBy(p => p.Date).ToList();


            return View(materialBuyPayment);
        }

        // POST: MaterialBuy/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditEditPayment(int id, int paymentid, MaterialBuyPaymentData materialBuyPaymentData)
        {
            if (Session["LoginUserID"] == null)
                return RedirectToAction("Login", "Account");

            if (ModelState.IsValid)
            {
                //                db.Entry(materialBuyData).State = EntityState.Modified;
                MaterialBuy materialBuy = db.MaterialBuys.FirstOrDefault(m => m.ID == materialBuyPaymentData.MaterialBuy.ID);
                Payment payment = await db.Payments.FindAsync(paymentid);

                payment.Date = materialBuyPaymentData.Payment.Date;
                payment.Type = materialBuyPaymentData.Payment.Type;
                payment.Amount = materialBuyPaymentData.Payment.Amount;
                payment.AmountString = materialBuyPaymentData.Payment.AmountString;
                if (payment.NoteVn != null && !payment.NoteVn.Equals(materialBuyPaymentData.Payment.NoteVn))
                {
                    payment.NoteVn = materialBuyPaymentData.Payment.NoteVn;
                    payment.Translate = false;
                }
                if(materialBuyPaymentData.Payment.NoteVn != null)
                {
                    payment.NoteVn = materialBuyPaymentData.Payment.NoteVn;
                    payment.Translate = false;
                }
                if (payment.NoteVn == null || payment.NoteVn.Length <= 0)
                    payment.Translate = true;

                await db.SaveChangesAsync();
                return RedirectToAction("EditAddPayment", new { id = materialBuyPaymentData.MaterialBuy.ID });
            }
            return View(materialBuyPaymentData);
        }

        public async Task<ActionResult> EditDetailsPayment(int? id, int paymentid)
        {
            if (Session["LoginUserID"] == null)
                return RedirectToAction("Login", "Account");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MaterialBuy materialBuy = await db.MaterialBuys.FindAsync(id);
            if (materialBuy == null)
            {
                return HttpNotFound();
            }
            Payment payment = await db.Payments.FindAsync(paymentid);
            if (payment == null)
            {
                return HttpNotFound();
            }
            var materialBuyPayment = new MaterialBuyPaymentData();
            materialBuyPayment.MaterialBuy = materialBuy;
            materialBuyPayment.Payment = payment;

            materialBuyPayment.MaterialBuy.Payments = materialBuyPayment.MaterialBuy.Payments.OrderBy(p => p.Date).ToList();


            return View(materialBuyPayment);

        }

        public async Task<ActionResult> DeletePayment(int id, int paymentid)
        {
            Payment Payment = await db.Payments.FindAsync(paymentid);

            db.Payments.Remove(Payment);
            
            //PreDeleteUnit(id, paymentid);
            await db.SaveChangesAsync();
            return RedirectToAction("EditAddPayment", new { id = id });
        }

        public async Task<ActionResult> EditAddUnit(int? id)
        {
            if (Session["LoginUserID"] == null)
                return RedirectToAction("Login", "Account");

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
            if (Session["LoginUserID"] == null)
                return RedirectToAction("Login", "Account");

            if (ModelState.IsValid)
            {
                //                db.Entry(materialBuyData).State = EntityState.Modified;
                MaterialName mName = db.MaterialNames.FirstOrDefault(m => m.NameVn.Equals(materialBuyData.MaterialBuyUnit.MaterialUnitPrice.MaterialName.NameVn));
                if(mName != null)
                {
                    materialBuyData.MaterialBuyUnit.MaterialUnitPrice.MaterialNameID = mName.ID;
                    materialBuyData.MaterialBuyUnit.MaterialUnitPrice.MaterialName = mName;
                }
                else
                {
                    if (materialBuyData.MaterialBuyUnit.MaterialUnitPrice.MaterialName.NameKr == null)
                        materialBuyData.MaterialBuyUnit.MaterialUnitPrice.MaterialName.NameKr = materialBuyData.MaterialBuyUnit.MaterialUnitPrice.MaterialName.NameVn;
                    else if (materialBuyData.MaterialBuyUnit.MaterialUnitPrice.MaterialName.NameVn == null)
                        materialBuyData.MaterialBuyUnit.MaterialUnitPrice.MaterialName.NameVn = materialBuyData.MaterialBuyUnit.MaterialUnitPrice.MaterialName.NameKr;

                    db.MaterialNames.Add(materialBuyData.MaterialBuyUnit.MaterialUnitPrice.MaterialName);
                }
                MaterialBuy materialBuy = db.MaterialBuys.FirstOrDefault(m => m.ID == materialBuyData.MaterialBuy.ID);
                materialBuyData.MaterialBuyUnit.MaterialUnitPrice.CompanyID = materialBuy.CompanyID;
                materialBuyData.MaterialBuyUnit.MaterialUnitPrice.Date = materialBuy.Date;

                MaterialUnitPrice muPrice = db.MaterialUnitPrices.OrderBy(m => m.Date)
                    .FirstOrDefault(m => m.MaterialName.NameVn.Equals(materialBuyData.MaterialBuyUnit.MaterialUnitPrice.MaterialName.NameVn));

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

        public async Task<ActionResult> EditEditUnit(int id, int unitid)
        {
            if (Session["LoginUserID"] == null)
                return RedirectToAction("Login", "Account");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MaterialBuy materialBuy = await db.MaterialBuys.FindAsync(id);
            if (materialBuy == null)
            {
                return HttpNotFound();
            }
            MaterialBuyUnit materialBuyUnit = await db.MaterialBuyUnits.FindAsync(unitid);
            if (materialBuyUnit == null)
            {
                return HttpNotFound();
            }
            var materialBuyData = new MaterialBuyData();
            materialBuyData.MaterialBuy = materialBuy;
            materialBuyData.MaterialBuyUnit = materialBuyUnit;
            materialBuyData.MaterialBuy.MaterialBuyUnits = materialBuyData.MaterialBuy.MaterialBuyUnits.OrderBy(p => p.ID).ToList();


            return View(materialBuyData);
        }

        // POST: MaterialBuy/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditEditUnit(int id, int unitid, MaterialBuyData materialBuyData)
        {
            if (Session["LoginUserID"] == null)
                return RedirectToAction("Login", "Account");

            if (ModelState.IsValid)
            {
                //                db.Entry(materialBuyData).State = EntityState.Modified;
                MaterialBuyUnit sUnit = db.MaterialBuyUnits.FirstOrDefault(mbu => mbu.ID == unitid);
                MaterialName sName = db.MaterialNames.FirstOrDefault(m => m.NameVn.Equals(sUnit.MaterialUnitPrice.MaterialName.NameVn));
                int  nameCount = db.MaterialUnitPrices.Where(m => m.MaterialName.NameVn.Equals(sUnit.MaterialUnitPrice.MaterialName.NameVn)).Count();

                MaterialName newName = db.MaterialNames.FirstOrDefault(m => m.NameVn.Equals(materialBuyData.MaterialBuyUnit.MaterialUnitPrice.MaterialName.NameVn));

                if (!materialBuyData.MaterialBuyUnit.MaterialUnitPrice.MaterialName.NameVn.Equals(sName.NameVn))
                {
                    if( nameCount == 1 )
                    {
                        if( newName == null)
                        {
                            sUnit.MaterialUnitPrice.MaterialName.NameVn = materialBuyData.MaterialBuyUnit.MaterialUnitPrice.MaterialName.NameVn;
                            sUnit.MaterialUnitPrice.MaterialName.NameKr = materialBuyData.MaterialBuyUnit.MaterialUnitPrice.MaterialName.NameVn;
                            materialBuyData.MaterialBuyUnit.MaterialUnitPrice.MaterialName.Translate = false;
                        }
                        else
                        {
                            db.MaterialNames.Remove(sUnit.MaterialUnitPrice.MaterialName);
                            sUnit.MaterialUnitPrice.MaterialName = newName;
                        }
                    }
                    else
                    {
                        if( newName == null )
                        {
                            if (materialBuyData.MaterialBuyUnit.MaterialUnitPrice.MaterialName.NameKr == null)
                                materialBuyData.MaterialBuyUnit.MaterialUnitPrice.MaterialName.NameKr = materialBuyData.MaterialBuyUnit.MaterialUnitPrice.MaterialName.NameVn;
                            else if (materialBuyData.MaterialBuyUnit.MaterialUnitPrice.MaterialName.NameVn == null)
                                materialBuyData.MaterialBuyUnit.MaterialUnitPrice.MaterialName.NameVn = materialBuyData.MaterialBuyUnit.MaterialUnitPrice.MaterialName.NameKr;

                            sUnit.MaterialUnitPrice.MaterialName = materialBuyData.MaterialBuyUnit.MaterialUnitPrice.MaterialName;
                            db.MaterialNames.Add(materialBuyData.MaterialBuyUnit.MaterialUnitPrice.MaterialName);
                        }
                        else
                        {
                            sUnit.MaterialUnitPrice.MaterialName = newName;
                        }
                    }
                }

                MaterialBuy materialBuy = db.MaterialBuys.FirstOrDefault(m => m.ID == materialBuyData.MaterialBuy.ID);
                MaterialUnitPrice muPrice = sUnit.MaterialUnitPrice;
                muPrice.UnitString = materialBuyData.MaterialBuyUnit.MaterialUnitPrice.UnitString;
                muPrice.Price = materialBuyData.MaterialBuyUnit.MaterialUnitPrice.Price;
                sUnit.Quantity = materialBuyData.MaterialBuyUnit.Quantity;

                await db.SaveChangesAsync();
                return RedirectToAction("EditAddUnit", new { id = id });
            }
            return View(materialBuyData);
        }

        // GET: MaterialBuy/Create
        public ActionResult Create()
        {
            if (Session["LoginUserID"] == null)
                return RedirectToAction("Login", "Account");

//            ViewBag.CompanyID = new SelectList(db.Companys, "ID", "Name");
            ViewBag.ProjectID = new SelectList(db.Projects.OrderByDescending( p => p.Date), "ID", "NameVn");
            return View();
        }

        // POST: MaterialBuy/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,Company,ProjectID,Date,NoteVn,NoteKr,VAT,EmployeeID,Translate")] MaterialBuy materialBuy)
        {
            if (Session["LoginUserID"] == null)
                return RedirectToAction("Login", "Account");

            if (ModelState.IsValid)
            {
                int eID = Convert.ToInt32(Session["LoginUserEmployeeID"]);
                materialBuy.Employee = db.Employees.FirstOrDefault(e => e.ID == eID);
                materialBuy.EmployeeID = eID;

                materialBuy.Company = db.Companys.FirstOrDefault(c => c.Name.Equals(materialBuy.Company.Name));
                materialBuy.CompanyID = materialBuy.Company.ID;

                if ((materialBuy.NoteVn != null && materialBuy.NoteVn.Length > 0) ||
                    (materialBuy.NoteKr != null && materialBuy.NoteKr.Length > 0))
                    materialBuy.Translate = false;
                else
                    materialBuy.Translate = true;

                db.MaterialBuys.Add(materialBuy);
                await db.SaveChangesAsync();

                return RedirectToAction("EditAddUnit", new { id = materialBuy.ID });
            }

 //           ViewBag.CompanyID = new SelectList(db.Companys, "ID", "Name", materialBuy.CompanyID);
            ViewBag.ProjectID = new SelectList(db.Projects.OrderByDescending(p => p.Date), "ID", "NameVn", materialBuy.ProjectID);
            return View(materialBuy);
        }

        // GET: MaterialBuy/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (Session["LoginUserID"] == null)
                return RedirectToAction("Login", "Account");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MaterialBuy materialBuy = await db.MaterialBuys.FindAsync(id);
            if (materialBuy == null)
            {
                return HttpNotFound();
            }

//            ViewBag.CompanyID = new SelectList(db.Companys, "ID", "Name", materialBuy.CompanyID);
            ViewBag.ProjectID = new SelectList(db.Projects.OrderByDescending(p => p.Date), "ID", "NameVn", materialBuy.ProjectID);
            return View(materialBuy);
        }

        // POST: MaterialBuy/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,Company,ProjectID,Date,NoteVn,NoteKr,VAT,Translate")] MaterialBuy materialBuy)
        {
            if (Session["LoginUserID"] == null)
                return RedirectToAction("Login", "Account");

            if (ModelState.IsValid)
            {
                MaterialBuy smb = await db.MaterialBuys.FindAsync(materialBuy.ID);

                smb.Company = db.Companys.FirstOrDefault(c => c.Name.Equals(materialBuy.Company.Name));
                smb.CompanyID = smb.Company.ID;

                smb.Project = db.Projects.FirstOrDefault(p => p.ID == materialBuy.ProjectID);
                smb.ProjectID = smb.Project.ID;
                smb.Date = materialBuy.Date;
                smb.NoteVn = materialBuy.NoteVn;
                smb.NoteKr = materialBuy.NoteKr;
                smb.VAT = materialBuy.VAT;
                smb.Translate = materialBuy.Translate;


                db.Entry(smb).State = EntityState.Modified;
                await db.SaveChangesAsync();
//                return RedirectToAction("Index");
                return RedirectToAction("Index", new { translate = Session["Translate"] });
            }
//            ViewBag.CompanyID = new SelectList(db.Companys, "ID", "Name", materialBuy.CompanyID);
            ViewBag.ProjectID = new SelectList(db.Projects.OrderByDescending(p => p.Date), "ID", "NameVn", materialBuy.ProjectID);
            return View(materialBuy);
        }

        [HttpPost]
        public JsonResult Edit2(string Prefix)
        {
            var names = db.MaterialNames.Where(m => m.NameVn.ToLower().Contains(Prefix.ToLower())).Select(m => new { m.NameVn }).ToList();
            return Json(names, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SelectUnit(string Prefix)
        {
            var names = db.MaterialUnitPrices.Where(m => m.UnitString.ToLower().Contains(Prefix.ToLower())).Select(m => new { m.UnitString }).Distinct().ToList();
            return Json(names, JsonRequestBehavior.AllowGet);
        }

        // GET: MaterialBuy/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (Session["LoginUserID"] == null)
                return RedirectToAction("Login", "Account");

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
            if (Session["LoginUserID"] == null)
                return RedirectToAction("Login", "Account");

            MaterialBuy materialBuy = await db.MaterialBuys.FindAsync(id);
            if(materialBuy.MaterialBuyUnits.Count > 0)
            {
                var mtu = db.MaterialBuyUnits.Where(m => m.MaterialBuyID == id).ToArray();
                
                foreach (var item in mtu)
                {
                    //db.MaterialBuyUnits.Remove(item);
                    //await DeleteUnit(materialBuy.ID, item.ID);
                    PreDeleteUnit(materialBuy.ID, item.ID);
                }
            }

            if (materialBuy.Payments.Count > 0)
            {
                var mtp = db.Payments.Where(p => p.MaterialBuyID == id).ToArray();

                foreach (var item in mtp)
                {
                    db.Payments.Remove(item);
                }
            }

            db.MaterialBuys.Remove(materialBuy);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> DeleteUnit(int id, int unitid)
        {
            /*
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
            */
            PreDeleteUnit(id, unitid);
            await db.SaveChangesAsync();
            return RedirectToAction("EditAddUnit", new { id = id });
        }

        public void PreDeleteUnit(int id, int unitid)
        {
            MaterialBuyUnit materialBuyUnit = db.MaterialBuyUnits.Find(unitid);
            int unitcount = db.MaterialBuyUnits.Count(m => m.MaterialUnitPriceID == materialBuyUnit.MaterialUnitPriceID);
            if (unitcount <= 1)
            {
                int namecount = db.MaterialUnitPrices.Count(m => m.MaterialNameID == materialBuyUnit.MaterialUnitPrice.MaterialNameID);
                if (namecount <= 1)
                    db.MaterialNames.Remove(materialBuyUnit.MaterialUnitPrice.MaterialName);

                db.MaterialUnitPrices.Remove(materialBuyUnit.MaterialUnitPrice);
            }

            db.MaterialBuyUnits.Remove(materialBuyUnit);
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
