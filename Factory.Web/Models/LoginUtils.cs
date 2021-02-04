using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;

namespace Factory.Web.Models
{
    public class LoginUtils
    {
        FactoryDBEntities db = new FactoryDBEntities();
        public bool IsAuthenticated(user userData)
        {
            var result = db.users.Where(user => user.UserName == userData.UserName && user.Pwd == userData.Pwd);
            if (result.Count() == 0)
            {
                return false;
            }
            return true;
        }
        public string GetUserFullName(string userName)
        {
            return db.users.FirstOrDefault(user => user.UserName == userName).FullName;
        }

        //user actions logic
        // find currUser Logged
        public user GetUser(string userName)
        {
            return db.users.FirstOrDefault(u => u.UserName == userName);
        }
        // Check if the user still hace actions
        public int GetUpdateActionsForUser(string userName)
        {
            var selectedUser = GetUser(userName);
            if (selectedUser.ActionsCounter != 0)
            {
                return selectedUser.ActionsCounter;
            } else
            {
                return -1;
            }
        }
        public void UpdateActions(string userName)
        {
            int dateNow = DateTime.Now.DayOfYear;

            if (CheckIfDifferentDay(userName))
            {
                var currentUser = GetUser(userName);
                //reset userdata
                currentUser.ActionsCounter = currentUser.Actions;
                currentUser.LastActionDay = dateNow;
                db.SaveChanges();
            }
/*            else
            {
                // or maybe nothing
                UpdateActionCounter(userName);
            }*/

        }
        // update ActionCounter--;
        public void UpdateActionCounter(string userName)
        {
            var currentUser = GetUser(userName);
            currentUser.ActionsCounter = currentUser.ActionsCounter - 1;
            db.SaveChanges();
        } 
        // Check if logged in another day
        public bool CheckIfDifferentDay(string userName)
        {
            int dateNow = DateTime.Now.DayOfYear;
            var currentUserLastActionDay = GetUser(userName).LastActionDay;

            if (dateNow - currentUserLastActionDay > 0)
            {
                return true;
            }
            return false;
        }
    }
}