using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ChulWoo.DAL;
using ChulWoo.Helper;
using ChulWoo.Models;
using ChulWoo.Viewmodel;

namespace ChulWoo.Controllers
{
    public class HomeController : BaseController
    {
        private ChulWooContext db = new ChulWooContext();

        public ActionResult Index()
        {
            if(Session["LoginUserID"] == null)
                return RedirectToAction("Login", "Account");

            DateTime lastLogin = (DateTime)Session["LastLogin"];

            ViewBag.newBoard = db.Boards.Count(b => b.Date >= lastLogin);
            ViewBag.newPersonnel = db.Personnels.Count(p => p.SendDate >= lastLogin);
            ViewBag.newFactory = db.FactoryDailyWorks.Count(f => f.Date >= lastLogin);
            ViewBag.newMaterialBuy = db.MaterialBuys.Count(m => m.Date >= lastLogin);
            ViewBag.newProject = db.Projects.Count(p => p.Date >= lastLogin);
            ViewBag.newPayment = db.Payments.Count(p => p.Date >= lastLogin);

            ViewBag.tranBoard = db.Boards.Count(b => b.Translate == false);
            ViewBag.tranEmployee = db.Employees.Count(e => e.Translate == false);
            ViewBag.tranPersonnel = db.Personnels.Count(p => p.Translate == false);
            ViewBag.tranFactory = db.FactoryDailyWorks.Count(f => f.Translate == false);
            ViewBag.tranMaterialBuy = db.MaterialBuys.Count(m => m.Translate == false);
            ViewBag.tranProject = db.Projects.Count(p => p.Translate == false);
            ViewBag.tranMaterialName = db.MaterialNames.Count(m => m.Translate == false);
            ViewBag.tranPayment = db.Payments.Count(m => m.Translate == false);
            ViewBag.tranDailyWorkReport = db.DailyWorks.Count(d => d.Translate == false);
            ViewBag.tranFactoryWorkUnit = db.FactoryWorkUnits.Count(w => w.Translate == false);
            ViewBag.tranWorkUnit = db.WorkUnits.Count(w => w.Translate == false);
            ViewBag.tranEquipmentUnit = db.EquipmentUnits.Count(e => e.Translate == false);

            var homeInfoData = new HomeInfoData();
            homeInfoData.Personnels = db.Personnels.Where(p => p.StartDate <= DateTime.Today && p.EndDate >= DateTime.Today).ToList();

            homeInfoData.WorkUnits = db.WorkUnits.Where(w => w.Complete == false || (w.EndDate != null && w.EndDate >= DateTime.Today)).ToList();

            return View(homeInfoData);
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