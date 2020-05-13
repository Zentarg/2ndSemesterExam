using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonLibrary.Models;

namespace AdministratorApp.Models
{
    public static class CommonMethods
    {
        public static Role GetRole(int roleId, Dictionary<int, Role> roles)
        {
            return roles[roleId];
        }

        public static Salary GetSalary(int userId, Dictionary<int, Salary> salaries)
        {
            try
            {
                return salaries[userId];
            }
            catch (Exception e)
            {
                return new Salary(userId, 0, 0);
            }
        }

        public static string SetErrorTextOnDelete(Constants.UserDeleteErorrs errors)
        {
            if (errors == Constants.UserDeleteErorrs.NO_SELECTED_USER)
                return "You must first select a user\nbefore trying to delete one";
            if (errors == Constants.UserDeleteErorrs.USER_LOGGED_IN)
                return "You cannot delete a logged\nin user";
            if (errors == Constants.UserDeleteErorrs.DELETE_OWNER)
                return "You cannot delete the owner\nin the system";
            if (errors == Constants.UserDeleteErorrs.LOW_ACCESS_LEVEL)
                return "You cannot delete users who\nhave the same or higher access level";
            if (errors == Constants.UserDeleteErorrs.DELETE_ID_0)
                return "You cannot delete the dummy\nuser from the system";
            return "";

        }
    }
}
