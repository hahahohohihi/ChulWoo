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
using System.IO;
using PagedList;

namespace ChulWoo.Controllers
{
    public class BoardController : Controller
    {
        private ChulWooContext db = new ChulWooContext();

        // GET: Board
        public async Task<ActionResult> Index(int? page, string searchString)
        {
            if (Session["LoginUserID"] == null)
                return RedirectToAction("Login", "Account");

            var boards = db.Boards.Include(b => b.Employee)
                .OrderByDescending(b => b.Date);
//            return View(await boards.ToListAsync());

            if(!String.IsNullOrEmpty(searchString))
            {

            }

            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(boards.ToPagedList(pageNumber, pageSize));
        }

        // GET: Board/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (Session["LoginUserID"] == null)
                return RedirectToAction("Login", "Account");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Board board = await db.Boards.FindAsync(id);
            if (board == null)
            {
                return HttpNotFound();
            }
            return View(board);
        }

        // GET: Board/Create
        public ActionResult Create()
        {
            if (Session["LoginUserID"] == null)
                return RedirectToAction("Login", "Account");

            ViewBag.EmployeeID = new SelectList(db.Employees, "ID", "Name");
            return View();
        }

        // POST: Board/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,EmployeeID,TitleVn,TitleKr,NoteVn,NoteKr,Date,UploadFiles")] Board board)
        {
            if (Session["LoginUserID"] == null)
                return RedirectToAction("Login", "Account");

            if (ModelState.IsValid)
            {
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
                            SaveFileName = Guid.NewGuid()+"_"+fileName,
                            FolderName = "Board",
                        };
                        UploadFiles.Add(uploadFile);

                        var path = Path.Combine(Server.MapPath("~/UploadFile/"), uploadFile.FolderName+"/"+uploadFile.SaveFileName);
                        file.SaveAs(path);
                    }
                }

                board.UploadFiles = UploadFiles;
                board.Date = (DateTime)DateTime.Now;
                board.EmployeeID = (int)Session["LoginUserEmployeeID"];
                db.Boards.Add(board);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.EmployeeID = new SelectList(db.Employees, "ID", "Name", board.EmployeeID);
            return View(board);
        }

        // GET: Board/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (Session["LoginUserID"] == null)
                return RedirectToAction("Login", "Account");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Board board = await db.Boards.FindAsync(id);
            if (board == null)
            {
                return HttpNotFound();
            }
            ViewBag.EmployeeID = new SelectList(db.Employees, "ID", "Name", board.EmployeeID);
            return View(board);
        }

        // POST: Board/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,EmployeeID,TitleVn,TitleKr,NoteVn,NoteKr,Date,UploadFiles")] Board board)
        {
            if (Session["LoginUserID"] == null)
                return RedirectToAction("Login", "Account");

            Board sboard = await db.Boards.FindAsync(board.ID);
            if (sboard != null && ModelState.IsValid)
            {
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
                            FolderName = "Board",
                        };
                        var path = Path.Combine(Server.MapPath("~/UploadFile/"), uploadFile.FolderName + "/" + uploadFile.SaveFileName);
                        file.SaveAs(path);

                        db.Entry(uploadFile).State = EntityState.Added;

                        sboard.UploadFiles.Add(uploadFile);
                    }
                }

                sboard.TitleKr = board.TitleKr;
                sboard.TitleVn = board.TitleVn;
                sboard.NoteKr = board.NoteKr;
                sboard.NoteVn = board.NoteVn;

                db.Entry(sboard).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.EmployeeID = new SelectList(db.Employees, "ID", "Name", board.EmployeeID);
            return View(board);
        }

        // GET: Board/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (Session["LoginUserID"] == null)
                return RedirectToAction("Login", "Account");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Board board = await db.Boards.FindAsync(id);
            if (board == null)
            {
                return HttpNotFound();
            }
            return View(board);
        }

        // POST: Board/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            if (Session["LoginUserID"] == null)
                return RedirectToAction("Login", "Account");

            try
            {
                Board board = await db.Boards.FindAsync(id);
                if (board == null)
                {
                    return HttpNotFound();
                }

                //delete files from the file system

                foreach (var item in board.UploadFiles)
                {
                    String path = Path.Combine(Server.MapPath("~/UploadFile/"), item.FolderName + "/" + item.SaveFileName);
                    if (System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);
                    }
                }

                db.Boards.Remove(board);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
 //               return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public FileResult Download(String SFilename, String Filename)
        {
            return File(Path.Combine(Server.MapPath("~/UploadFile/"), SFilename), System.Net.Mime.MediaTypeNames.Application.Octet, Filename);
        }


        [HttpPost]
        public JsonResult DeleteFile(string id)
        {
            if (String.IsNullOrEmpty(id))
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new { Result = "Error" });
            }
            try
            {
                UploadFile UploadFile = db.UploadFiles.Find(Int32.Parse(id));
                if (UploadFile == null)
                {
                    Response.StatusCode = (int)HttpStatusCode.NotFound;
                    return Json(new { Result = "Error" });
                }

                //Remove from database
                db.UploadFiles.Remove(UploadFile);
                db.SaveChanges();

                //Delete file from the file system
                var path = Path.Combine(Server.MapPath("~/UploadFile/"), UploadFile.FolderName+"/" + UploadFile.SaveFileName);
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
                return Json(new { Result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
    }
}
