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
using System.IO;
using System.Web.UI;
using System.Text;

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
                .OrderByDescending(m => m.ID).OrderByDescending(m => m.Date);

            if (!String.IsNullOrEmpty(searchString))
                materialBuys = (IOrderedQueryable<MaterialBuy>)materialBuys.Where(m => m.Company.Name.Contains(searchString) || 
                m.Project.NameKr.Contains(searchString) || m.Project.NameVn.Contains(searchString) ||
                m.MaterialBuyUnits.Count(mu => mu.MaterialUnitPrice.MaterialName.NameKr.Contains(searchString) || mu.MaterialUnitPrice.MaterialName.NameVn.Contains(searchString)) > 0);

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
                materialBuyPaymentData.Payment.CompanyID = materialBuyPaymentData.MaterialBuy.CompanyID;
                materialBuyPaymentData.Payment.ProjectID = materialBuyPaymentData.MaterialBuy.ProjectID;

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
                payment.CompanyID = materialBuyPaymentData.MaterialBuy.CompanyID;
                payment.ProjectID = materialBuyPaymentData.MaterialBuy.ProjectID;

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
                MaterialName mName = db.MaterialNames.FirstOrDefault(m => m.NameVn.Equals(materialBuyData.MaterialBuyUnit.MaterialUnitPrice.MaterialName.NameVn) ||
                    m.NameVn.Equals(materialBuyData.MaterialBuyUnit.MaterialUnitPrice.MaterialName.NameKr));
                if(mName != null)
                {
                    materialBuyData.MaterialBuyUnit.MaterialUnitPrice.MaterialNameID = mName.ID;
                    materialBuyData.MaterialBuyUnit.MaterialUnitPrice.MaterialName = mName;
                }
                else
                {
                    if (materialBuyData.MaterialBuyUnit.MaterialUnitPrice.MaterialName.NameKr == null)
                        materialBuyData.MaterialBuyUnit.MaterialUnitPrice.MaterialName.NameKr = materialBuyData.MaterialBuyUnit.MaterialUnitPrice.MaterialName.NameVn;

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
            if(Convert.ToInt32(Session["CurrentCulture"]) == 2)
                ViewBag.ProjectID = new SelectList(db.Projects.OrderByDescending( p => p.Date), "ID", "NameKr");
            else
                ViewBag.ProjectID = new SelectList(db.Projects.OrderByDescending(p => p.Date), "ID", "NameVn");
            return View();
        }

        // POST: MaterialBuy/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,Company,ProjectID,Date,NoteVn,NoteKr,VAT,VATPer,EmployeeID,Translate")] MaterialBuy materialBuy)
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
            if (Convert.ToInt32(Session["CurrentCulture"]) == 2)
                ViewBag.ProjectID = new SelectList(db.Projects.OrderByDescending(p => p.Date), "ID", "NameKr", materialBuy.ProjectID);
            else
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
            if (Convert.ToInt32(Session["CurrentCulture"]) == 2)
                ViewBag.ProjectID = new SelectList(db.Projects.OrderByDescending(p => p.Date), "ID", "NameKr", materialBuy.ProjectID);
            else
                ViewBag.ProjectID = new SelectList(db.Projects.OrderByDescending(p => p.Date), "ID", "NameVn", materialBuy.ProjectID);
            return View(materialBuy);
        }

        // POST: MaterialBuy/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,Company,ProjectID,Date,NoteVn,NoteKr,VAT,VATPer,Translate")] MaterialBuy materialBuy)
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
                smb.VATPer = materialBuy.VATPer;
                smb.Translate = materialBuy.Translate;


                db.Entry(smb).State = EntityState.Modified;
                await db.SaveChangesAsync();
//                return RedirectToAction("Index");
                return RedirectToAction("Index", new { translate = Session["Translate"] });
            }
//            ViewBag.CompanyID = new SelectList(db.Companys, "ID", "Name", materialBuy.CompanyID);
            if (Convert.ToInt32(Session["CurrentCulture"]) == 2)
                ViewBag.ProjectID = new SelectList(db.Projects.OrderByDescending(p => p.Date), "ID", "NameKr", materialBuy.ProjectID);
            else
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

        public void ExportClientsListToExcel(DateTime SearchDate)
        {
            byte[] buffer = null;

            IEnumerable<Payment> payments = db.Payments.Include(p => p.MaterialBuy)
                .Include(p => p.Employee)
                .Include(p => p.Company)
                .Where(p => p.Date == SearchDate && p.Type == PaymentType.Bank)
                .OrderByDescending(p => p.Date);

            try
            {
                StringBuilder str = new StringBuilder();
                str.Append("<html xmlns:x=\"urn: schemas - microsoft - com:office: excel\">");
                str.Append("<head>");
                str.Append("<meta http-equiv=\"Content - Type\" content=\"text / html; charset = utf - 8\" />");
                str.Append("<style>");
                str.Append("body { font-family: Arial Unicode MS, Arial }");
                str.Append("</style>");
                str.Append("</head>");
                str.Append("<body>");
                str.Append("<h3 style = \"text-align: center;\"> DANH SÁCH THANH TOÁN CÔNG TY TNHH CHULWOO VINA NGÀY ");
                str.Append(@DateTime.Now.Day);
                str.Append(" THÁNG ");
                str.Append(@DateTime.Now.Month);
                str.Append(" NĂM ");
                str.Append(@DateTime.Now.Year);
                str.Append("</h3 >");
                str.Append("<br />");




                // HTML 테이블 생성

                str.Append("<table>");

                str.Append("<tr>");

                str.Append("<td>aaa</td>");
                str.Append("</tr>");

                //..~~~~


                str.Append("</table>");
                str.Append("</body>");
                str.Append("</html>");

                HttpContext.Response.AddHeader("content-disposition", "attachment; filename=Exported_Diners.xls");
                HttpContext.Response.ContentType = "application/vnd.ms-excel";
                HttpContext.Response.Charset = "UTF-8";
                HttpContext.Response.ContentEncoding = System.Text.Encoding.Default;
                buffer = System.Text.Encoding.UTF8.GetBytes(str.ToString());



                Response.Write(str);
                Response.End();

            }
            catch (Exception e)
            {

            }

//            return File(buffer, "application/vnd.ms-excel");



        }

        public void ExportClientsListToExcel2(DateTime SearchDate)
        {
            var grid = new System.Web.UI.WebControls.GridView();

            //grid.DataSource = from d in dbContext.diners
            //where d.user_diners.All(m => m.user_id == userID) && d.active == true 
            grid.DataSource = from d in db.MaterialBuys.ToList()
            select new
            {
                Date = d.Date,
                NoteVn = d.NoteVn,
                NoteKr = d.NoteKr,
                VAT = d.VAT,
                VATPer = d.VATPer
            };

            grid.DataBind();

            IEnumerable<Payment> payments = db.Payments.Include(p => p.MaterialBuy)
                .Include(p => p.Employee)
                .Include(p => p.Company)
                .Where(p => p.Date == SearchDate && p.Type == PaymentType.Bank)
                .OrderByDescending(p => p.Date);


            Response.ClearContent();
            Response.AddHeader("content-disposition", "attachment; filename=Exported_Diners"+SearchDate.ToString()+".xls");
            Response.ContentType = "application/excel";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);

//            grid.RenderControl(htw);

            htw.Write("<html xmlns:x=\"urn: schemas - microsoft - com:office: excel\">");
            htw.Write("<head>");
            htw.Write("<meta http-equiv=\"Content - Type\" content=\"text / html; charset = utf - 8\" />");
            htw.Write("<style>");
            htw.Write("body { font-family: Arial Unicode MS, Arial }");
            htw.Write("</style>");
            htw.Write("</head>");
            htw.Write("<body>");
            htw.Write("<h3 style = \"text-align: center;\">DANH SÁCH THANH TOÁN CÔNG TY TNHH CHULWOO VINA NGÀY {0} THÁNG {1} NĂM {2}</h3 >", 
                @DateTime.Now.Day, @DateTime.Now.Month, @DateTime.Now.Year);
            htw.Write("<table border=\"1\">");
            htw.Write("<tr style=\"background - color: #FFF; font-weight: bold; padding: 5px\">");
            htw.Write("<th width=\"3%\">Stt(1)</th>");
            htw.Write("<th width=\"15%\">Số tài khoản(2)</th>");
            htw.Write("<th width=\"6%\">Đơn vị tiền(3)</th>");
            htw.Write("<th width=\"22%\">TÊN NGƯỜI HƯỞNG(4)</th>");
            htw.Write("<th width=\"10%\">SỐ TIỀN(5)</th>");
            htw.Write("<th width=\"22%\">NỘI DUNG(6)</th>");
            htw.Write("<th width=\"22%\">TẠI NGÂN HÀNG(7)</th>");
            htw.Write("</tr>");

            int count = 1;
            double total = 0;
            foreach ( var pay in payments)
            {
                htw.Write("<tr valign=\"top\" style=\"background - color: White; \">");
                htw.Write("<td style=\"text-align: center;\">{0}</td>", count);
                htw.Write("<td style=\"text-align: center;\">{0}</td>", pay.Company.BankAccount);
                htw.Write("<td style=\"text-align: center;\">VND</td>");
                htw.Write("<td>{0}</td>", pay.Company.Name);
                htw.Write("<td style=\"text-align: right;\">{0:0,0}</td>", pay.Amount);
                htw.Write("<td>{0}</td>", pay.NoteVn);
                htw.Write("<td>{0}</td>", pay.Company.BankLocation);
                htw.Write("</tr>");
                count++;
                total += pay.Amount;
            }
            htw.Write("<tr>" +
                "<th></th>" +
                "<td></td>" +
                "<td></td>" +
                "<th>TỔNG CỘNG</th>" +
                "<th>{0:0,0}</th>" +
                "<td></td>" +
                "<td></td>" +
                "</tr>", total);
            htw.Write("<tr>" +
                "<td colspan=\"2\">GHI CHÚ:</td>" +
                "<td></td>" +
                "<td></td>" +
                "<td></td>" +
                "<td></td>" +
                "<td></td>" +
                "</tr>");
            htw.Write("</table>");
            htw.Write("<br>");
            htw.Write("<p>");
            htw.Write("1. Cột (2) ghi số tài khoản hoặc số chứng minh thư của người hưởng<br />");
            htw.Write("2. Cột (6) ghi nội dung thanh toán (nếu có)<br />");
            htw.Write("3. Cột (7) ghi rõ tên chi nhánh ngân hàng hưởng đối với trường hợp tài khoản Người hưởng thuộc ngân hàngkhác. <br />");
            htw.Write("Trường hợp, người hưởng có tài khoản tại VietcomBank cột này bỏ trống");
            htw.Write("</p>");
            htw.Write("<br />");
            htw.Write("<table width=\"100%\" border=\"0\">");
            htw.Write("<tr>");
            htw.Write("<th width=\"35%\" colspan=\"3\">KẾ TOÁN</th>");
            htw.Write("<th width=\"30%\" colspan=\"2\"></th>");
            htw.Write("<th width=\"35%\" colspan=\"2\">THỦ TRƯỞNG ĐƠN VỊ</th>");
            htw.Write("</tr>");
            htw.Write("<tr>");
            htw.Write("<th height=\"80\"></th>");
            htw.Write("<th></th>");
            htw.Write("<th></th>");
            htw.Write("<th></th>");
            htw.Write("<th></th>");
            htw.Write("<th></th>");
            htw.Write("<th></th>");
            htw.Write("</tr>");
            htw.Write("<tr>");
            htw.Write("<th colspan=\"3\">Phạm Thị Phương</th>");
            htw.Write("<th></th>");
            htw.Write("<th></th>");
            htw.Write("<th></th>");
            htw.Write("<th></th>");
            htw.Write("</tr>");
            htw.Write("</table>");
            htw.Write("</body>");
            htw.Write("</html>");


            Response.Write(sw.ToString());
//            Response.Write(this.View().View.ToString());

            Response.End();
        }
/*
        protected void BtnDownLoadExcel_Click(object sender, EventArgs e)
        {

            //다운로드 되도록 헤더 설정
            Response.Clear();
            Response.AddHeader("content-disposition", "attachment;filename=DataGrid.xls");
            Response.ContentType = "application/vnd.xls";

            System.IO.StringWriter stringWriter = new System.IO.StringWriter();
            System.Web.UI.HtmlTextWriter htmlWriter = new HtmlTextWriter(stringWriter);

            //한글 정상표시되도록 
            Response.Charset = "UTF-8";
            Response.ContentEncoding = System.Text.Encoding.Default;

            //페이징 되어있을경우 전체리스트가 출력되지 않으므로 잠시 중지

            GridView.AllowPaging = false;
            GridView.DataBind();
            EnableViewState = false;

            //아래 주석처리된 부분은 페이지상에 <%%>를 써서 렌더링 하는 부분이 없을때만 동작합니다.
            //마스터페이지 이용시 오류를 피해가는 방법입니다.
            //HtmlForm frm = new HtmlForm();
            //Controls.Add(frm);
            //frm.Controls.Add(GridView);
            //frm.RenderControl(htmlWriter);
            // 엑셀에서 자동 숫자변환 안되도록 하는 스타일
            string strStyle = @"<style>td { mso-number-format:\@; } </style>";

            GridView.RenderControl(htmlWriter);
            // 엑셀에서 자동 숫자변환 안되도록 스타일 적용
            Response.Write(strStyle);
            Response.Write(stringWriter.ToString());
            Response.End();

            //현재 페이징으로 복귀.
            GridView.AllowPaging = true;
            GridView.DataBind();
        }

        protected void BtnDownLoadExcel_Click(object sender, EventArgs e)
        {
        Response.ClearHeaders();
        string fileName = "파일명[" + DateTime.Today.ToString().Substring(0, 10) + "].xls";
        Response.ContentType = "application/vnd.ms-excel";
        Response.AddHeader("Content-Disposition", "attachment;fileName=" + fileName);
        Response.Charset = "euc-kr";
 
        Button1.Visible = false;
 
        System.IO.StringWriter tw = new System.IO.StringWriter();
        System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(tw);
        String convStr = this.View.hw Table1.ToString();
         
        Response.Write(convStr);
    }
    */




    public ActionResult PaymentList(DateTime? SearchDate)
        {
            if (Session["LoginUserID"] == null)
                return RedirectToAction("Login", "Account");

            PaymentListData PaymentList = new PaymentListData();

            if (SearchDate == null )
            {
                PaymentList.SearchDate = DateTime.Today;
                PaymentList.Payments = db.Payments.Include(p => p.Employee).Include(p => p.MaterialBuy)
                    .Where(p => p.Date == PaymentList.SearchDate)
                    .OrderByDescending(p => p.Date);
            }
            else
            {
                PaymentList.SearchDate = (DateTime)SearchDate;
                PaymentList.Payments = db.Payments.Include(p => p.Employee).Include(p => p.MaterialBuy)
                        .Where(p => p.Date == SearchDate)
                        .OrderByDescending(p => p.Date);
            }
            return View(PaymentList);
        }

        public ActionResult PaymentListDetails(DateTime SearchDate)
        {
            if (Session["LoginUserID"] == null)
                return RedirectToAction("Login", "Account");

            PaymentListData PaymentList = new PaymentListData();

            PaymentList.SearchDate = DateTime.Today;
            PaymentList.Payments = db.Payments.Include(p => p.MaterialBuy)
                .Include(p => p.Employee)
                .Include(p => p.Company)
                .Where(p => p.Date == SearchDate && p.Type == PaymentType.Bank)
                .OrderByDescending(p => p.Date);

            return View(PaymentList);
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
