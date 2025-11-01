using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using A2Q3.Models;

namespace A2Q3.Controllers
{
    public class EmpsController : Controller
    {
        private Emp_10Entities db = new Emp_10Entities();

        // GET: Emps
        public ActionResult Index()
        {
            var emps = db.Emps.Include(e => e.dept1).Include(e => e.desg);
            return View(emps.ToList());
        }

        // GET: Emps/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Emp emp = db.Emps.Find(id);
            if (emp == null)
            {
                return HttpNotFound();
            }
            return View(emp);
        }

        // GET: Emps/Create
        public ActionResult Create()
        {
            ViewBag.dept = new SelectList(db.depts, "dept_id", "dept_name");
            ViewBag.designation = new SelectList(db.desgs, "desg_id", "desg_name");
            return View();
        }

        // POST: Emps/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EmpNo,name,dob,designation,dept,salary")] Emp emp)
        {
            if (ModelState.IsValid)
            {
                db.Emps.Add(emp);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.dept = new SelectList(db.depts, "dept_id", "dept_name", emp.dept);
            ViewBag.designation = new SelectList(db.desgs, "desg_id", "desg_name", emp.designation);
            return View(emp);
        }

        // GET: Emps/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Emp emp = db.Emps.Find(id);
            if (emp == null)
            {
                return HttpNotFound();
            }
            ViewBag.dept = new SelectList(db.depts, "dept_id", "dept_name", emp.dept);
            ViewBag.designation = new SelectList(db.desgs, "desg_id", "desg_name", emp.designation);
            return View(emp);
        }

        // POST: Emps/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EmpNo,name,dob,designation,dept,salary")] Emp emp)
        {
            if (ModelState.IsValid)
            {
                db.Entry(emp).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.dept = new SelectList(db.depts, "dept_id", "dept_name", emp.dept);
            ViewBag.designation = new SelectList(db.desgs, "desg_id", "desg_name", emp.designation);
            return View(emp);
        }

        // GET: Emps/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Emp emp = db.Emps.Find(id);
            if (emp == null)
            {
                return HttpNotFound();
            }
            return View(emp);
        }

        // POST: Emps/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Emp emp = db.Emps.Find(id);
            db.Emps.Remove(emp);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // DELETE BY NAME
        public ActionResult DeleteByName()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteByName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                ViewBag.Message = "Please enter employee name.";
                return View();
            }

            var emp = db.Emps.FirstOrDefault(e => e.name == name);

            if (emp == null)
            {
                ViewBag.Message = "Employee not found!";
                return View();
            }

            db.Emps.Remove(emp);
            db.SaveChanges();

            ViewBag.Message = $"Employee '{name}' deleted successfully!";
            return View();
        }

        // ✅ INCREMENT SALARY - GET
        public ActionResult IncrementSalary()
        {
            ViewBag.DeptList = new SelectList(db.depts, "dept_id", "dept_name");
            return View();
        }

        [HttpPost]
        public ActionResult IncrementSalary(IncrementViewModel model)
        {
            var employees = db.Emps.Where(e => e.dept == model.Dept_id).ToList();

            if (!employees.Any())
            {
                ViewBag.Message = "⚠ No employees in selected department.";
            }
            else
            {
                foreach (var emp in employees)
                {
                    emp.salary += emp.salary * (model.IncrementPercent / 100);
                }

                db.SaveChanges();
                ViewBag.Message = "✅ Salary increment applied successfully!";
            }

            ViewBag.DeptList = new SelectList(db.depts, "dept_id", "dept_name");
            return View(model);
        }
        // SEARCH BY SALARY - GET
        public ActionResult SearchBySalary()
        {
            return View();
        }

        // SEARCH BY SALARY - POST
        [HttpPost]
        public ActionResult SearchBySalary(decimal salary)
        {
            var result = db.Emps.Where(e => e.salary > salary).ToList();

            if (!result.Any())
            {
                ViewBag.Message = $"No employees found with salary greater than {salary}";
                return View();
            }

            return View(result);
        }
        // SEARCH EMPLOYEE BY DESIGNATION - GET
        public ActionResult SearchByDesignation()
        {
            ViewBag.DesignationList = new SelectList(db.desgs, "desg_id", "desg_name");
            return View();
        }

        // SEARCH EMPLOYEE BY DESIGNATION - POST
        [HttpPost]
        public ActionResult SearchByDesignation(int designation)
        {
            ViewBag.DesignationList = new SelectList(db.desgs, "desg_id", "desg_name");

            var result = db.Emps.Where(e => e.designation == designation)
                                .Include(e => e.dept1)
                                .Include(e => e.desg)
                                .ToList();

            if (!result.Any())
            {
                ViewBag.Message = "No employees found for selected designation!";
                return View();
            }

            return View(result);
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
