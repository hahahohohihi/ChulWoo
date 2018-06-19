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
using System.Data.Entity.Infrastructure;

namespace ChulWoo.Controllers 
{
    public class EmployeeController : Controller
    {
        private ChulWooContext db = new ChulWooContext();
         
        // GET: Employee
        public async Task<ActionResult> Index()
        {
            var employees = db.Employees.Include(e => e.Resign)
                .Include(e => e.Contracts.Select(c => c.Employee));
            return View(employees);
//            return View(await db.Employees.ToListAsync());
        }

        // GET: Employee/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = await db.Employees.FindAsync(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // GET: Employee/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Employee/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,DepartmentVn,DepartmentKr,Name,EmployeeNo,BankAccount,BankLocation,TexNo,JobPosition,Sex,BirthDate,RegistrationNo,RegistrationDate,RegistrationPosition,Tel1,Tel2,Email,Adress,People,Religion,Country,EducationLevel,MajorVn,MajorKr,Marriage,DependentChild,DependentParents")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                db.Employees.Add(employee);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(employee);
        }

        // GET: Employee/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = await db.Employees.FindAsync(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // POST: Employee/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int? id, byte[] rowVersion)
        {
            string[] fieldsToBind = new string[] { "DepartmentVn", "DepartmentKr", "Name", "EmployeeNo", "BankAccount", "BankLocation", "TexNo", "JobPosition",
                "Sex", "BirthDate", "RegistrationNo", "RegistrationDate", "RegistrationPosition", "Tel1", "Tel2", "Email", 
                "Adress", "People", "Religion", "Country", "EducationLevel", "MajorVn", "MajorKr", "Marriage", "DependentChild", "DependentParents" };

            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var employeeToUpdate = await db.Employees.FindAsync(id);
            if( employeeToUpdate == null )
            {
                Employee deleteEmployee = new Employee();
                TryUpdateModel(deleteEmployee, fieldsToBind);
                ModelState.AddModelError(string.Empty, "Delete by another user.");
                return View(deleteEmployee);
            }

            if( TryUpdateModel(employeeToUpdate, fieldsToBind) )
            {
                try
                {
                    db.Entry(employeeToUpdate).OriginalValues["RowVersion"] = rowVersion;
                    await db.SaveChangesAsync();

                    return RedirectToAction("Index");
                }
                catch(DbUpdateConcurrencyException ex)
                {
                    var entry = ex.Entries.Single();
                    var clientValues = (Employee)entry.Entity;
                    var databaseEntry = entry.GetDatabaseValues();
                    if( databaseEntry == null )
                    {
                        ModelState.AddModelError(string.Empty, "Delete by another user.");
                    }
                    else
                    {
                        var databaseValues = (Employee)databaseEntry.ToObject();
                        if (databaseValues.DepartmentVn != clientValues.DepartmentVn)
                            ModelState.AddModelError("DepartmentVn", "Current value: " + databaseValues.DepartmentVn);
                        if (databaseValues.DepartmentKr != clientValues.DepartmentKr)
                            ModelState.AddModelError("DepartmentKr", "Current value: " + databaseValues.DepartmentKr);
                        if (databaseValues.Name != clientValues.Name)
                            ModelState.AddModelError("Name", "Current value: " + databaseValues.Name);
                        if (databaseValues.EmployeeNo != clientValues.EmployeeNo)
                            ModelState.AddModelError("EmployeeNo", "Current value: " + databaseValues.EmployeeNo);
                        if (databaseValues.BankAccount != clientValues.BankAccount)
                            ModelState.AddModelError("BankAccount", "Current value: " + databaseValues.BankAccount);
                        if (databaseValues.BankLocation != clientValues.BankLocation)
                            ModelState.AddModelError("BankLocation", "Current value: " + databaseValues.BankLocation);
                        if (databaseValues.TexNo != clientValues.TexNo)
                            ModelState.AddModelError("TexNo", "Current value: " + databaseValues.TexNo);
                        if (databaseValues.JobPosition != clientValues.JobPosition)
                            ModelState.AddModelError("JobPosition", "Current value: " + databaseValues.JobPosition);
                        if (databaseValues.Sex != clientValues.Sex)
                            ModelState.AddModelError("Sex", "Current value: " + databaseValues.Sex);
                        if (databaseValues.BirthDate != clientValues.BirthDate)
                            ModelState.AddModelError("BirthDate", "Current value: " + String.Format("{0:d}", databaseValues.BirthDate));
                        if (databaseValues.RegistrationNo != clientValues.RegistrationNo)
                            ModelState.AddModelError("RegistrationNo", "Current value: " + databaseValues.RegistrationNo);
                        if (databaseValues.RegistrationDate != clientValues.RegistrationDate)
                            ModelState.AddModelError("RegistrationDate", "Current value: " + String.Format("{0:d}", databaseValues.RegistrationDate));
                        if (databaseValues.RegistrationPosition != clientValues.RegistrationPosition)
                            ModelState.AddModelError("RegistrationPosition", "Current value: " + databaseValues.RegistrationPosition);
                        if (databaseValues.Tel1 != clientValues.Tel1)
                            ModelState.AddModelError("Tel1", "Current value: " + databaseValues.Tel1);
                        if (databaseValues.Tel2 != clientValues.Tel2)
                            ModelState.AddModelError("Tel2", "Current value: " + databaseValues.Tel2);
                        if (databaseValues.Email != clientValues.Email)
                            ModelState.AddModelError("Email", "Current value: " + databaseValues.Email);
                        if (databaseValues.Adress != clientValues.Adress)
                            ModelState.AddModelError("Adress", "Current value: " + databaseValues.Adress);
                        if (databaseValues.People != clientValues.People)
                            ModelState.AddModelError("People", "Current value: " + databaseValues.People);
                        if (databaseValues.Religion != clientValues.Religion)
                            ModelState.AddModelError("Religion", "Current value: " + databaseValues.Religion);
                        if (databaseValues.Country != clientValues.Country)
                            ModelState.AddModelError("Country", "Current value: " + databaseValues.Country);
                        if (databaseValues.EducationLevel != clientValues.EducationLevel)
                            ModelState.AddModelError("EducationLevel", "Current value: " + databaseValues.EducationLevel);
                        if (databaseValues.MajorVn != clientValues.MajorVn)
                            ModelState.AddModelError("MajorVn", "Current value: " + databaseValues.MajorVn);
                        if (databaseValues.MajorKr != clientValues.MajorKr)
                            ModelState.AddModelError("MajorKr", "Current value: " + databaseValues.MajorKr);
                        if (databaseValues.Marriage != clientValues.Marriage)
                            ModelState.AddModelError("Marriage", "Current value: " + databaseValues.Marriage);
                        if (databaseValues.DependentChild != clientValues.DependentChild)
                            ModelState.AddModelError("DependentChild", "Current value: " + databaseValues.DependentChild);
                        if (databaseValues.DependentParents != clientValues.DependentParents)
                            ModelState.AddModelError("DependentParents", "Current value: " + databaseValues.DependentParents);

                        ModelState.AddModelError(string.Empty, "The record you attempted to edit "
                            + "was modified by another user after you got the original value. The "
                            + "edit operation was canceled and the current values in the database "
                            + "have been displayed. If you still want to edit this record, click "
                            + "the Save button again. Otherwise click the Back to List hyperlink.");
                        employeeToUpdate.RowVersion = databaseValues.RowVersion;
                    }
                }
                catch (RetryLimitExceededException /* dex */)
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }
            return View(employeeToUpdate);
        }

        // GET: Employee/Delete/5
        public async Task<ActionResult> Delete(int? id, bool? concurrencyError)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Employee employee = await db.Employees.FindAsync(id);
            if (employee == null)
            {
                if (concurrencyError.GetValueOrDefault())
                {
                    return RedirectToAction("Index");
                }
                return HttpNotFound();
            }

            if (concurrencyError.GetValueOrDefault())
            {
                ViewBag.ConcurrencyErrorMessage = "The record you attempted to delete "
                    + "was modified by another user after you got the original values. "
                    + "The delete operation was canceled and the current values in the "
                    + "database have been displayed. If you still want to delete this "
                    + "record, click the Delete button again. Otherwise "
                    + "click the Back to List hyperlink.";
            }

            return View(employee);
        }

        // POST: Employee/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(Employee employee)
        {
            try
            {
                db.Entry(employee).State = EntityState.Deleted;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch ( DbUpdateConcurrencyException )
            {
                return RedirectToAction("Delete", new { concurrencyError = true, id = employee.ID });
            }
            catch (DataException /* dex */)
            {
                //Log the error (uncomment dex variable name after DataException and add a line here to write a log.
                ModelState.AddModelError(string.Empty, "Unable to delete. Try again, and if the problem persists contact your system administrator.");
                return View(employee);
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
    }
}
