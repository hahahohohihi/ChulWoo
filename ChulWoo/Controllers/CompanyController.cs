﻿using System;
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
using PagedList;

namespace ChulWoo.Controllers
{
    public class CompanyController : BaseController
    {
        private ChulWooContext db = new ChulWooContext();

        // GET: Company
        public async Task<ActionResult> Index(int? page, string currentFilter, string searchString)
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

            if (!String.IsNullOrEmpty(searchString))
            {
                var companys = db.Companys.Include(c => c.Projects.Select(p => p.Company))
                    .Include(c => c.MaterialBuys.Select(mb => mb.Project))
                    .Where(c => c.Name.Contains(searchString))
                    .OrderBy(c => c.Name);
                return View(companys.ToPagedList(pageNumber, pageSize));
            }
            else
            {
                var companys = db.Companys.Include(c => c.Projects.Select(p => p.Company))
                .Include(c => c.MaterialBuys.Select(mb => mb.Project))
                .OrderBy(c => c.Name);
                return View(companys.ToPagedList(pageNumber, pageSize));
            }

            //            return View(await db.Companys.ToListAsync());
        }

        // GET: Company/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (Session["LoginUserID"] == null)
                return RedirectToAction("Login", "Account");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

//            Company company = await db.Companys.FindAsync(id);
            var company = db.Companys.Include(c => c.Projects.Select(p => p.Company))
                .Include(c => c.MaterialBuys.Select(mb => mb.Project))
                .Where(c => c.ID == id)
                .Single();
            company.Projects = company.Projects.OrderByDescending(m => m.Date).ToList();
            company.MaterialBuys = company.MaterialBuys.OrderByDescending(m => m.Date).ToList();

            if (company == null)
            {
                return HttpNotFound();
            }
            return View(company);
        }

        // GET: Company/Create
        public ActionResult Create()
        {
            if (Session["LoginUserID"] == null)
                return RedirectToAction("Login", "Account");

            return View();
        }

        // POST: Company/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,Name,Address,Tel,Texcode")] Company company)
        {
            if (Session["LoginUserID"] == null)
                return RedirectToAction("Login", "Account");

            if (ModelState.IsValid)
            {
                db.Companys.Add(company);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(company);
        }

        // GET: Company/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (Session["LoginUserID"] == null)
                return RedirectToAction("Login", "Account");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Company company = await db.Companys.FindAsync(id);
            if (company == null)
            {
                return HttpNotFound();
            }
            return View(company);
        }

        // POST: Company/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,Name,Address,Tel,Texcode")] Company company)
        {
            if (Session["LoginUserID"] == null)
                return RedirectToAction("Login", "Account");

            if (ModelState.IsValid)
            {
                db.Entry(company).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(company);
        }

        // GET: Company/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (Session["LoginUserID"] == null)
                return RedirectToAction("Login", "Account");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Company company = await db.Companys.FindAsync(id);
            if (company == null)
            {
                return HttpNotFound();
            }
            return View(company);
        }

        // POST: Company/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            if (Session["LoginUserID"] == null)
                return RedirectToAction("Login", "Account");

            Company company = await db.Companys.FindAsync(id);
            db.Companys.Remove(company);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        [HttpPost]
        public JsonResult SetNameList(string Prefix)
        {
            var names = db.Companys.Where(c => c.Name.ToLower().Contains(Prefix.ToLower())).Select(c => new { c.Name }).ToList();
            return Json(names, JsonRequestBehavior.AllowGet);
        }
    }
}
