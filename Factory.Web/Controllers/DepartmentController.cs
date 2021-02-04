using Factory.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace Factory.Web.Controllers
{
    public class DepartmentController : Controller
    {
        LoginUtils loginBL = new LoginUtils();
        DepartmentUtils departmentBL = new DepartmentUtils();
        EmployeeUtils employeeBL = new EmployeeUtils();

        // GET: Department
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
                } else
                {
                    var model = departmentBL.GetAll();
                    foreach(var item in model)
                    {
                        item.isExist = departmentBL.IsEmployeeInDepartment(item.DepId);
                    }
                    return View(model);
                }
            }
            return RedirectToAction("Index", "Login");
        }
        [HttpGet]
        public ActionResult Edit(int depId)
        {
                var userName = Session["userName"] as string;
            loginBL.UpdateActionCounter(userName);
            var amountActions = loginBL.GetUpdateActionsForUser(userName);
            Session["AmountOfActions"] = amountActions;
            if (amountActions < 0)
            {
                Session.Clear();
                TempData["ErrorMessage"] = "Logout out by the system! user doesn't have actions";
                return RedirectToAction("Index", "Login");
            }
            else
            {
                var model = departmentBL.GetById(depId);
                ViewBag.employees = employeeBL.GetEmployees();
                if (model == null)
                {
                    return View("NotFound");
                }
                return View(model);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(department dep)
        {
            if (ModelState.IsValid)
            {
                departmentBL.Update(dep);
                return RedirectToAction("Index");
            } else
            {
                TempData["ErrorMessage"] = "Department Name is Required!";
                return RedirectToAction("Edit", new { depId = dep.ID });
            }
        }
        [HttpGet]
        public ActionResult Add()
        {
            var userName = Session["userName"] as string;
            loginBL.UpdateActionCounter(userName);
            var amountActions = loginBL.GetUpdateActionsForUser(userName);
            Session["AmountOfActions"] = amountActions;
            if (amountActions < 0)
            {
                Session.Clear();
                TempData["ErrorMessage"] = "Logout out by the system! user doesn't have actions";
                return RedirectToAction("Index", "Login");
            }
            ViewBag.employees = employeeBL.GetEmployees();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(department department)
        {
            if (ModelState.IsValid)
            {

                departmentBL.Add(department);
                return RedirectToAction("Index");
            } else
            {
                TempData["ErrorMessage"] = "Department Name is Required!";
                return RedirectToAction("Add");
            }
        }
        [HttpGet]
        public ActionResult Delete(int depId)
        {
            var userName = Session["userName"] as string;
            loginBL.UpdateActionCounter(userName);
            var amountActions = loginBL.GetUpdateActionsForUser(userName);
            Session["AmountOfActions"] = amountActions;
            if (amountActions < 0)
            {
                Session.Clear();
                TempData["ErrorMessage"] = "Logout out by the system! user doesn't have actions";
                return RedirectToAction("Index", "Login");
            } else { 
                var model = departmentBL.GetById(depId);
                if (model == null)
                {
                    return View("NotFound");
                }
                ViewBag.ManagerName = employeeBL.GetEmployeeName((int)model.Manager);
                return View(model);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int depId, FormCollection form)
        {
            departmentBL.Delete(depId);
            return RedirectToAction("Index");
        }
    }
}