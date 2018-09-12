using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ChulWoo.Models;
using ChulWoo.DAL;
using System.Data.Entity;

namespace ChulWoo.Controllers
{
    public class AccountController : BaseController
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
            if( ModelState.IsValid )
            {
                var user = db.Users.FirstOrDefault(u => u.UserID.Equals(model.UserID));
                if( user == null )
                {
                    ModelState.AddModelError(string.Empty, "Notthing UserID");
                }
                else
                {
                    if(user.UserPassword == null || user.UserPassword.Equals(""))
                        return RedirectToAction("Register", "Account", model);
                    else if( user.UserPassword.Equals(model.UserPassword))
                    {
                        Session["LoginUserID"] = user.ID;
                        Session["LoginUserEmployeeID"] = user.EmployeeID;
                        Session["LoginUserEmployeeName"] = user.Employee.Name;
                        Session["LoginUserSecurity"] = user.Security;

                        user.LastLogin = DateTime.Now;


                        db.Entry(user).State = EntityState.Modified;
                        db.SaveChanges();

//                        return RedirectToAction("Index", "Home");
                        return RedirectToAction("ChangeCurrentCulture", "Home", new { id = user.Language });
                    }
                    else
                        ModelState.AddModelError(string.Empty, "Recheck UserPassword");
                }
            }
            return View(model);
        }

        public ActionResult Logout()
        {
            Session.Remove("LoginUserID");
            Session.Remove("LoginUserEmployeeID");
            Session.Remove("LoginUserEmployeeName");
            Session.Remove("LoginUserSecurity");

            return RedirectToAction("Index", "Home");
        }

        public ActionResult Register(User model)
        {
            return View(model);
        }


        [HttpPost, ActionName("Register")]
        public ActionResult RegisterConfirmed(User model)
        {
            if( ModelState.IsValid )
            {
                User user = db.Users.FirstOrDefault(u => u.UserID.Equals(model.UserID));
                user.UserPassword = model.UserPassword;
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index", "Home");
            }
            return View(model);
        }
    }
}