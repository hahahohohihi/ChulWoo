using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ChulWoo.Models;
using ChulWoo.DAL;

namespace ChulWoo.Controllers
{
    public class AccountController : Controller
    {
        private ChulWooContext db = new ChulWooContext();

        // GET: Account
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(User model)
        {
            return View();
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(User model)
        {
            if( ModelState.IsValid )
            {
                db.Users.Add(model);
                db.SaveChanges();

                return RedirectToAction("Index", "Home");
            }
            return View(model);
        }
    }
}