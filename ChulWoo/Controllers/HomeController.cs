using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ChulWoo.DAL;
using ChulWoo.Models;

namespace ChulWoo.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if(Session["LoginUserID"] == null)
                return RedirectToAction("Login", "Account");
            return View();
        }

        [HttpPost]
        public JsonResult About(string Prefix)
        {
            using( ChulWooContext db = new ChulWooContext() )
            {
                var names = db.MaterialNames.Where(m => m.Name.ToLower().StartsWith(Prefix.ToLower())).Select( m => new { m.Name } ).ToList();
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
    }
}