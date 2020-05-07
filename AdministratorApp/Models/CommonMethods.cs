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
            return salaries[userId];
        }
    }
}
