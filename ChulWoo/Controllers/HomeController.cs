using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ChulWoo.DAL;
using ChulWoo.Helper;
using ChulWoo.Models;

namespace ChulWoo.Controllers
{
    public class HomeController : BaseController
    {
        private ChulWooContext db = new ChulWooContext();

        public ActionResult Index()
        {
            if(Session["LoginUserID"] == null)
                return RedirectToAction("Login", "Account");

            int userid = (int)Session["LoginUserID"];
            var user = db.Users.FirstOrDefault(u => u.ID == userid);

            ViewBag.newBoard = db.Boards.Count(b => b.Date >= user.LastLogin);
            ViewBag.newPersonnel = db.Personnels.Count(p => p.SendDate >= user.LastLogin);
            ViewBag.newFactory = db.FactoryDailyWorks.Count(f => f.Date >= user.LastLogin);
            ViewBag.newMaterialBuy = db.MaterialBuys.Count(m => m.Date >= user.LastLogin);
            ViewBag.newProject = db.Projects.Count(p => p.Date >= user.LastLogin);

            ViewBag.tranBoard = db.Boards.Count(b => b.Translate == false);
            ViewBag.tranEmployee = db.Employees.Count(e => e.Translate == false);
            ViewBag.tranPersonnel = db.Personnels.Count(p => p.Translate == false);
            ViewBag.tranFactory = db.FactoryDailyWorks.Count(f => f.Translate == false);
            ViewBag.tranMaterialBuy = db.MaterialBuys.Count(m => m.Translate == false);
            ViewBag.tranProject = db.Projects.Count(p => p.Translate == false);
            ViewBag.tranMaterialName = db.MaterialNames.Count(m => m.Translate == false);
            ViewBag.tranPayment = db.Payments.Count(m => m.Translate == false);
            ViewBag.tranDailyWorkReport = db.DailyWorks.Count(d => d.Translate == false);
            ViewBag.tranWorkUnit = db.WorkUnits.Count(w => w.Translate == false);
            ViewBag.tranEquipmentUnit = db.EquipmentUnits.Count(e => e.Translate == false);

            var personnels = db.Personnels.Where(p => p.StartDate <= DateTime.Today && p.EndDate >= DateTime.Today);

            return View(personnels);
        }

        [HttpPost]
        public JsonResult About(string Prefix)
        {
            using( ChulWooContext db = new ChulWooContext() )
            {
                var names = db.MaterialNames.Where(m => m.NameVn.ToLower().StartsWith(Prefix.ToLower())).Select( m => new { m.NameVn } ).ToList();
                return Json(names, JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult ChangeCurrentCulture(int id)
        {
            //  
            // Change the current culture for this user.  
            //  
            CultureHelper.CurrentCulture = id;
            //  
            // Cache the new current culture into the user HTTP session.   
            //  
            Session["CurrentCulture"] = id;

            int SID = Convert.ToInt32(Session["LoginUserID"]);
            if( SID > 0 )
            {
                User user = db.Users.FirstOrDefault( u => u.ID == SID );
                user.Language = id;
                db.SaveChanges();
            }

            //  
            // Redirect to the same page from where the request was made!   \
            //  
            if ( Request.UrlReferrer != null && !Request.UrlReferrer.LocalPath.Equals("/Account/Login") )
                return Redirect(Request.UrlReferrer.ToString());
            else
                return RedirectToAction("Index", "Home");
        }
    }
}