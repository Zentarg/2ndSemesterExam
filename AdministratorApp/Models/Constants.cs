using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdministratorApp.Models
{
    public static class Constants
    {
        public const string LOGIN_ERROR = "The credentials did not match any existing accounts.";

        public enum AccessLevels {
            Employee = 0, Manager = 1, Administrator = 2, Owner = 3
        }

}
}
