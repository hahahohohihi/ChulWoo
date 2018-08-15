﻿using System;
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
            //  
            // Redirect to the same page from where the request was made!   \
            //  
            if( Request.UrlReferrer != null )
                return Redirect(Request.UrlReferrer.ToString());
            else
                return RedirectToAction("Index", "Home");
        }
    }
}