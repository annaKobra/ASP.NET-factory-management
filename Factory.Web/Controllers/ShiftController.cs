using Factory.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Factory.Web.Controllers
{
    public class ShiftController : Controller
    {
        ShiftUtils shiftBL = new ShiftUtils();
        EmployeeShiftUtils employeeShiftBL = new EmployeeShiftUtils();
        LoginUtils loginBL = new LoginUtils();
        // GET: Shift
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
                    ViewBag.shifts = shiftBL.GetAll();
                    ViewBag.employeeShifts = employeeShiftBL.GetShiftsForEmployee();
                    return View();
                }
            }
            return RedirectToAction("Index", "Login");
        }
        [HttpGet]
        public ActionResult Add()
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
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(shift newShift)
        {
            if(ModelState.IsValid)
            {
                shiftBL.Add(newShift);
                return RedirectToAction("Index");
            } else
            {
                TempData["ErrorMessage"] = "All the Fields are Required!";
                return RedirectToAction("Add");
            }
        }
    }
}