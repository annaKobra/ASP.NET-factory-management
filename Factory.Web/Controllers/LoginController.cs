using Factory.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Factory.Web.Controllers
{
    public class LoginController : Controller
    {
        LoginUtils loginBL = new LoginUtils();
        // GET: Login
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(user user)
        {
            bool authenticated = loginBL.IsAuthenticated(user);
            if (authenticated)
            {
                Session["authenticated"] = true;
                // present user full name in nav-bar
                string userFullName = loginBL.GetUserFullName(user.UserName);
                Session["userFullName"] = userFullName;

                // all controllers will check action for selected uesr
                Session["userName"] = user.UserName;
                //update dataBase for currentUser
                loginBL.UpdateActions(user.UserName);

                // present user actions in nav-bar
                var selectedUser = loginBL.GetUser(user.UserName);
                Session["AmountOfActions"] = selectedUser.ActionsCounter;

                // user used all of his actions
                if (loginBL.GetUpdateActionsForUser(user.UserName) < 0)
                {
                    Session.Clear();
                    TempData["ErrorMessage"] = "Logout by the system! user doesn't have actions";
                    return RedirectToAction("Index");
                } else
                {
                    return View("HomePage");
                }
            }
            else
            {
                Session["authenticated"] = false;
                TempData["ErrorMessage"] = "Login Failed! Such a user doesn't exist on the site";
                return RedirectToAction("Index");
            }
        }
        // Added for the Pages as link to navigate to -> HomePage
        public ActionResult GetHomePage()
        {
            if (Session["authenticated"] != null)
            {
                return View("HomePage");
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOut()
        {
            Session.Clear();
            return RedirectToAction("Index");
        }
    }
}