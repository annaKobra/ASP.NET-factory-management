using Factory.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Factory.Web.Controllers
{
    public class EmployeeController : Controller
    {
        EmployeeUtils employeeBL = new EmployeeUtils();
        EmployeeShiftUtils employeeShiftBL = new EmployeeShiftUtils();
        DepartmentUtils departmentBL = new DepartmentUtils();
        ShiftUtils shiftBL = new ShiftUtils();
        LoginUtils loginBL = new LoginUtils();
        // GET: Employee
        public ActionResult Index()
        {
            if (Session["authenticated"] != null && (bool)Session["authenticated"] == true)
            {
                // logic to count user actions
                var userName = Session["userName"] as string;
                loginBL.UpdateActionCounter(userName);
                var amountActions = loginBL.GetUpdateActionsForUser(userName);
                Session["AmountOfActions"] = amountActions;
                if (amountActions < 0)
                {
                    TempData["ErrorMessage"] = "Logout out by the system! user doesn't have actions";
                    Session.Clear();
                    return RedirectToAction("Index", "Login");
                }

                var serachedList = TempData["searchedList"] as List<EmployeeWithDepDTO>;
                if (serachedList != null)
                {
                    ViewBag.employees = serachedList;
                } else
                {
                    ViewBag.employees = employeeBL.GetEmployeesWithDepartment();
                }
                ViewBag.shifts = employeeShiftBL.GetShiftsForEmployee();
                return View();
            }
            return RedirectToAction("Index", "Login");
        }
        [HttpPost]
        public ActionResult Search(string phrase)
        {
            if (!String.IsNullOrEmpty(phrase))
            {
                TempData["searchedList"] = employeeBL.Search(phrase).ToList();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult Edit(int empId)
        {
            // logic to count user actions
            var userName = Session["userName"] as string;
            loginBL.UpdateActionCounter(userName);
            var amountActions = loginBL.GetUpdateActionsForUser(userName);
            Session["AmountOfActions"] = amountActions;
            if (amountActions < 0)
            {
                TempData["ErrorMessage"] = "Logout out by the system! user doesn't have actions";
                Session.Clear();
                return RedirectToAction("Index", "Login");
            }
            else
            {
                var model = employeeBL.GetById(empId);
                ViewBag.departments = departmentBL.GetAll();
                if (model == null)
                {
                    return View("NotFound");
                }
                return View(model);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(employee emp)
        {
            if (ModelState.IsValid)
            {
                employeeBL.Update(emp);
                return RedirectToAction("Index");
            } else
            {
                TempData["ErrorMessage"] = "All The Fields are Required!";
                return RedirectToAction("Edit", new { empId = emp.ID });
            }
        }
        [HttpGet]
        public ActionResult Delete(int empId)
        {
            // logic to count user actions
            var userName = Session["userName"] as string;
            loginBL.UpdateActionCounter(userName);
            var amountActions = loginBL.GetUpdateActionsForUser(userName);
            Session["AmountOfActions"] = amountActions;
            if (amountActions < 0)
            {
                TempData["ErrorMessage"] = "Logout out by the system! user doesn't have actions";
                Session.Clear();
                return RedirectToAction("Index", "Login");
            } else
            {
                var model = employeeBL.GetById(empId);
                ViewBag.depName = departmentBL.GetDepNameForEmp(model.DepID);
                if (model == null)
                {
                    return View("NotFound");
                }
                return View(model);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int empId, FormCollection form)
        {
            employeeBL.Delete(empId);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult AddShift(int empId)
        {
            // logic to count user actions
            var userName = Session["userName"] as string;
            loginBL.UpdateActionCounter(userName);
            var amountActions = loginBL.GetUpdateActionsForUser(userName);
            Session["AmountOfActions"] = amountActions;
            if (amountActions < 0)
            {
                TempData["ErrorMessage"] = "Logout out by the system! user doesn't have actions";
                Session.Clear();
                return RedirectToAction("Index", "Login");
            } else
            {
                ViewBag.employeeName = employeeBL.GetEmployeeName(empId);
                ViewBag.empId = empId;
                ViewBag.shifts = shiftBL.GetAll();
                return View();
            }
        }
        [HttpPost]
        public ActionResult AddShift(int empId, int shiftId)
        {
            employeeShiftBL.AddShift(empId, shiftId);
            return RedirectToAction("Index");
        }

    }
}