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
using System.IO;

namespace ChulWoo.Controllers
{
    public class DailyWorkController : BaseController
    {
        private ChulWooContext db = new ChulWooContext();

        // GET: DailyWork
        public async Task<ActionResult> Index(int? projectid)
        {
            if( projectid == null )
            {
                var dailyWorks = db.DailyWorks.Include(d => d.Project).Include(d => d.ProparingPerson)
                    .OrderByDescending(d => d.Date);
                return View(await dailyWorks.ToListAsync());
            }
            else
            {
                ViewBag.ProjectID = projectid;
                var dailyWorks = db.DailyWorks.Where(d => d.ProjectID == projectid)
                    .Include(d => d.Project).Include(d => d.ProparingPerson)
                    .OrderByDescending(d => d.Date);
                return View(await dailyWorks.ToListAsync());
            }
        }

        // GET: DailyWork/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DailyWork dailyWork = await db.DailyWorks.FindAsync(id);
            if (dailyWork == null)
            {
                return HttpNotFound();
            }
            return View(dailyWork);
        }

        // GET: DailyWork/Create
        public ActionResult Create(int? projectid)
        {
            DailyWork dailyWork = new DailyWork();
            if ( projectid == null )
                ViewBag.ProjectID = new SelectList(db.Projects, "ID", "NameVn");
            else
            {
                Project project = db.Projects.FirstOrDefault(p => p.ID == projectid);
                ViewBag.ProjectID = projectid;
                ViewBag.ProjectNameVn = project.NameVn;
                ViewBag.ProjectNameKr = project.NameKr;
                dailyWork.ProjectID = (int)projectid;
            }

            ViewBag.ProparingPersonID = new SelectList(db.Employees, "ID", "Name");
            return View(dailyWork);
        }

        // POST: DailyWork/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,ProjectID,ProparingPersonID,Date,PrrocessPerPlan,PrrocessPerResult,WeatherVn,WeatherKr,NoteVn,NoteKr,Translate,WorkUnits")] DailyWork dailyWork)
        {
            if (ModelState.IsValid)
            {
                //                dailyWork.Project.WorkUnits = db.WorkUnits.Where(w => w.DailyWorks.Count(d=>d.ProjectID == dailyWork.ProjectID)>0 && w.StartDate < dailyWork.Date && w.Complete == false).ToList();
                List<UploadFile> UploadFiles = new List<UploadFile>();
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    var file = Request.Files[i];

                    if (file != null && file.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(file.FileName);
                        UploadFile uploadFile = new UploadFile()
                        {
                            FileName = fileName,
                            SaveFileName = Guid.NewGuid() + "_" + fileName,
                            FolderName = "DailyWork",
                            Date = DateTime.Now
                        };
                        UploadFiles.Add(uploadFile);

                        var path = Path.Combine(Server.MapPath("~/UploadFile/"), uploadFile.FolderName + "/" + uploadFile.SaveFileName);
                        file.SaveAs(path);
                    }
                }

                dailyWork.UploadFiles = UploadFiles;

                db.DailyWorks.Add(dailyWork);
                await db.SaveChangesAsync();
//                return RedirectToAction("Index");
                return RedirectToAction("EditAddWork", new { id = dailyWork.ID });
            }

            ViewBag.ProjectID = new SelectList(db.Projects, "ID", "NameVn", dailyWork.ProjectID);
            ViewBag.ProparingPersonID = new SelectList(db.Employees, "ID", "Name", dailyWork.ProparingPersonID);
            return View(dailyWork);
        }

        // GET: DailyWork/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DailyWork dailyWork = await db.DailyWorks.FindAsync(id);
            if (dailyWork == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProjectID = new SelectList(db.Projects, "ID", "NameVn", dailyWork.ProjectID);
            ViewBag.ProparingPersonID = new SelectList(db.Employees, "ID", "Name", dailyWork.ProparingPersonID);
            ViewBag.WorkUnits = db.WorkUnits.Where(w => w.StartDate <= dailyWork.Date && (w.Complete == false || w.EndDate >= dailyWork.Date)).ToList();
            return View(dailyWork);
        }

        // POST: DailyWork/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,ProjectID,ProparingPersonID,Date,PrrocessPerPlan,PrrocessPerResult,WeatherVn,WeatherKr,NoteVn,NoteKr,UploadFiles,Translate")] DailyWork dailyWork)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dailyWork).State = EntityState.Modified;

                for (int i = 0; i < Request.Files.Count; i++)
                {
                    var file = Request.Files[i];

                    if (file != null && file.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(file.FileName);
                        UploadFile uploadFile = new UploadFile()
                        {
                            FileName = fileName,
                            SaveFileName = Guid.NewGuid() + "_" + fileName,
                            FolderName = "DailyWork",
                            Date = DateTime.Now
                        };
                        var path = Path.Combine(Server.MapPath("~/UploadFile/"), uploadFile.FolderName + "/" + uploadFile.SaveFileName);
                        file.SaveAs(path);

                        db.Entry(uploadFile).State = EntityState.Added;

                        if(dailyWork.UploadFiles == null)
                            dailyWork.UploadFiles = new List<UploadFile>();
                        dailyWork.UploadFiles.Add(uploadFile);
                    }
                }

                await db.SaveChangesAsync();
                return RedirectToAction("Index", new { projectid = dailyWork.ProjectID });
            }
            ViewBag.ProjectID = new SelectList(db.Projects, "ID", "NameVn", dailyWork.ProjectID);
            ViewBag.ProparingPersonID = new SelectList(db.Employees, "ID", "Name", dailyWork.ProparingPersonID);
            return View(dailyWork);
        }

        // GET: DailyWork/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DailyWork dailyWork = await db.DailyWorks.FindAsync(id);
            if (dailyWork == null)
            {
                return HttpNotFound();
            }
            return View(dailyWork);
        }

        // POST: DailyWork/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            DailyWork dailyWork = await db.DailyWorks.FindAsync(id);
            int projectID = dailyWork.ProjectID;
            db.DailyWorks.Remove(dailyWork);
            await db.SaveChangesAsync();
            return RedirectToAction("Index", new { projectid = projectID });
        }


        public async Task<ActionResult> EditAddWork(int? id)
        {
            if (Session["LoginUserID"] == null)
                return RedirectToAction("Login", "Account");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DailyWork dailyWork = await db.DailyWorks.FindAsync(id);
            if (dailyWork == null)
            {
                return HttpNotFound();
            }
            var dailyWorkUnitData = new DailyWorkUnitData();
            dailyWorkUnitData.DailyWork = dailyWork;

            ViewBag.WorkUnits = db.WorkUnits.Where(w => w.StartDate <= dailyWork.Date && (w.Complete == false || w.EndDate >= dailyWork.Date)).ToList();

            return View(dailyWorkUnitData);
        }

        // POST: MaterialBuy/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAddWork(DailyWorkUnitData dailyWorkUnitData)
        {
            if (Session["LoginUserID"] == null)
                return RedirectToAction("Login", "Account");

            if (ModelState.IsValid)
            {
                //                db.Entry(materialBuyData).State = EntityState.Modified;
                DailyWork dailyWork = db.DailyWorks.FirstOrDefault(m => m.ID == dailyWorkUnitData.DailyWork.ID);

                if (dailyWorkUnitData.WorkUnit.Complete == true)
                    dailyWorkUnitData.WorkUnit.EndDate = dailyWorkUnitData.DailyWork.Date;
                else
                    dailyWorkUnitData.WorkUnit.EndDate = null;

                db.WorkUnits.Add(dailyWorkUnitData.WorkUnit);
                dailyWork.Project.WorkUnits.Add(dailyWorkUnitData.WorkUnit);

                await db.SaveChangesAsync();
                return RedirectToAction("EditAddWork", new { id = dailyWorkUnitData.DailyWork.ID });
            }
            return View(dailyWorkUnitData);
        }

        public async Task<ActionResult> EditEditWork(int? id, int workid)
        {
            if (Session["LoginUserID"] == null)
                return RedirectToAction("Login", "Account");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DailyWork dailyWork = await db.DailyWorks.FindAsync(id);
            if (dailyWork == null)
            {
                return HttpNotFound();
            }
            WorkUnit workUnit = await db.WorkUnits.FindAsync(workid);
            if (workUnit == null)
            {
                return HttpNotFound();
            }
            var dailyWorkUnitData = new DailyWorkUnitData();
            dailyWorkUnitData.DailyWork = dailyWork;
            dailyWorkUnitData.WorkUnit = workUnit;

            ViewBag.WorkUnits = db.WorkUnits.Where(w => w.StartDate <= dailyWork.Date && (w.Complete == false || w.EndDate >= dailyWork.Date)).ToList();

            return View(dailyWorkUnitData);
        }

        // POST: MaterialBuy/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditEditWork(int id, int workid, DailyWorkUnitData dailyWorkUnitData)
        {
            if (Session["LoginUserID"] == null)
                return RedirectToAction("Login", "Account");

            if (ModelState.IsValid)
            {
                //                db.Entry(materialBuyData).State = EntityState.Modified;
                DailyWork dailyWork = db.DailyWorks.FirstOrDefault(m => m.ID == dailyWorkUnitData.DailyWork.ID);
                WorkUnit workUnit = await db.WorkUnits.FindAsync(workid);

                if ( workUnit.NoteVn != dailyWorkUnitData.WorkUnit.NoteVn)
                    workUnit.Translate = false;
                if(workUnit.Complete != dailyWorkUnitData.WorkUnit.Complete)
                {
                    if(dailyWorkUnitData.WorkUnit.Complete == true)
                        workUnit.EndDate = dailyWorkUnitData.DailyWork.Date;
                    else
                        workUnit.EndDate = null;
                }
                workUnit.StartDate = dailyWorkUnitData.WorkUnit.StartDate;
//                workUnit.EndDate = dailyWorkUnitData.WorkUnit.EndDate;
                workUnit.Complete = dailyWorkUnitData.WorkUnit.Complete;
                workUnit.WorkNameVn = dailyWorkUnitData.WorkUnit.WorkNameVn;
                workUnit.NoteVn = dailyWorkUnitData.WorkUnit.NoteVn;

                await db.SaveChangesAsync();
                return RedirectToAction("EditAddWork", new { id = dailyWorkUnitData.DailyWork.ID });
            }
            return View(dailyWorkUnitData);
        }

        public async Task<ActionResult> DeleteWorkUnit(int id, int Workid)
        {
            WorkUnit workUnit = await db.WorkUnits.FindAsync(Workid);

            db.WorkUnits.Remove(workUnit);

            //PreDeleteUnit(id, paymentid);
            await db.SaveChangesAsync();
            return RedirectToAction("EditAddWork", new { id = id });
        }

        public async Task<ActionResult> EditAddEquipment(int? id)
        {
            if (Session["LoginUserID"] == null)
                return RedirectToAction("Login", "Account");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DailyWork dailyWork = await db.DailyWorks.FindAsync(id);
            if (dailyWork == null)
            {
                return HttpNotFound();
            }
            var dailyWorkEquipmentData = new DailyWorkEquipmentData();
            dailyWorkEquipmentData.DailyWork = dailyWork;

            ViewBag.WorkUnits = db.WorkUnits.Where(w => w.StartDate <= dailyWork.Date && (w.Complete == false || w.EndDate >= dailyWork.Date)).ToList();

            return View(dailyWorkEquipmentData);
        }

        // POST: MaterialBuy/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAddEquipment(DailyWorkEquipmentData dailyWorkEquipmentData)
        {
            if (Session["LoginUserID"] == null)
                return RedirectToAction("Login", "Account");

            if (ModelState.IsValid)
            {
                //                db.Entry(materialBuyData).State = EntityState.Modified;
                DailyWork dailyWork = db.DailyWorks.FirstOrDefault(m => m.ID == dailyWorkEquipmentData.DailyWork.ID);

                dailyWorkEquipmentData.EquipmentUnit.DailyWorkID = dailyWorkEquipmentData.DailyWork.ID;
                dailyWorkEquipmentData.EquipmentUnit.Date = (DateTime)dailyWorkEquipmentData.DailyWork.Date;

                EquipmentUnit equipTemp = db.EquipmentUnits.FirstOrDefault(e => e.NameVn.Equals(dailyWorkEquipmentData.EquipmentUnit.NameVn));
                if( equipTemp != null )
                {
                    dailyWorkEquipmentData.EquipmentUnit.NameKr = equipTemp.NameKr;
                }
                

                db.EquipmentUnits.Add(dailyWorkEquipmentData.EquipmentUnit);
                dailyWork.EquipmentUnits.Add(dailyWorkEquipmentData.EquipmentUnit);

                await db.SaveChangesAsync();
                return RedirectToAction("EditAddEquipment", new { id = dailyWorkEquipmentData.DailyWork.ID });
            }
            return View(dailyWorkEquipmentData);
        }

        public async Task<ActionResult> EditEditEquipment(int? id, int equipmentid)
        {
            if (Session["LoginUserID"] == null)
                return RedirectToAction("Login", "Account");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DailyWork dailyWork = await db.DailyWorks.FindAsync(id);
            if (dailyWork == null)
            {
                return HttpNotFound();
            }
            EquipmentUnit equipmentUnit = await db.EquipmentUnits.FindAsync(equipmentid);
            if (equipmentUnit == null)
            {
                return HttpNotFound();
            }
            var dailyWorkEquipmentData = new DailyWorkEquipmentData();
            dailyWorkEquipmentData.DailyWork = dailyWork;
            dailyWorkEquipmentData.EquipmentUnit = equipmentUnit;

            dailyWorkEquipmentData.DailyWork.EquipmentUnits = dailyWorkEquipmentData.DailyWork.EquipmentUnits.OrderBy(p => p.ID).ToList();

            ViewBag.WorkUnits = db.WorkUnits.Where(w => w.StartDate <= dailyWork.Date && (w.Complete == false || w.EndDate >= dailyWork.Date)).ToList();

            return View(dailyWorkEquipmentData);
        }

        // POST: MaterialBuy/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditEditEquipment(int id, int equipmentid, DailyWorkEquipmentData dailyWorkEquipmentData)
        {
            if (Session["LoginUserID"] == null)
                return RedirectToAction("Login", "Account");

            if (ModelState.IsValid)
            {
                //                db.Entry(materialBuyData).State = EntityState.Modified;
                DailyWork dailyWork = db.DailyWorks.FirstOrDefault(m => m.ID == dailyWorkEquipmentData.DailyWork.ID);
                EquipmentUnit equipmentUnit = await db.EquipmentUnits.FindAsync(equipmentid);

                if (equipmentUnit.NoteVn != dailyWorkEquipmentData.EquipmentUnit.NoteVn)
                    equipmentUnit.Translate = false;
                equipmentUnit.Date = (DateTime)dailyWorkEquipmentData.DailyWork.Date;
                equipmentUnit.DailyWorkID = dailyWorkEquipmentData.DailyWork.ID;
                equipmentUnit.NameKr = dailyWorkEquipmentData.EquipmentUnit.NameKr;
                equipmentUnit.NameVn = dailyWorkEquipmentData.EquipmentUnit.NameVn;
                equipmentUnit.NoteVn = dailyWorkEquipmentData.EquipmentUnit.NoteVn;
                equipmentUnit.NoteKr = dailyWorkEquipmentData.EquipmentUnit.NoteKr;
                equipmentUnit.EquipCount = dailyWorkEquipmentData.EquipmentUnit.EquipCount;

                await db.SaveChangesAsync();
                return RedirectToAction("EditAddEquipment", new { id = dailyWorkEquipmentData.DailyWork.ID });
            }
            return View(dailyWorkEquipmentData);
        }

        public async Task<ActionResult> DeleteEquipment(int id, int equipmentid)
        {
            EquipmentUnit equipmentUnit = await db.EquipmentUnits.FindAsync(equipmentid);

            db.EquipmentUnits.Remove(equipmentUnit);

            //PreDeleteUnit(id, paymentid);
            await db.SaveChangesAsync();
            return RedirectToAction("EditAddEquipment", new { id = id });
        }

        public JsonResult SelectEquipmentUnit(string Prefix)
        {
            var names = db.EquipmentUnits.Where(m => m.NameVn.ToLower().Contains(Prefix.ToLower())).Select(m => new { m.NameVn }).Distinct().ToList();
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
