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

        public enum RoleErrors { OK = 0, ROLE_EXISTS = 1, INCORRECT_FORMAT = 2 }
        public enum UserDeleteErorrs { OK = 0, USER_LOGGED_IN = 1, NO_SELECTED_USER = 2, DELETE_OWNER = 3, LOW_ACCESS_LEVEL = 4, DELETE_ID_0 = 5 }

        public const string AllowedCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890ÁÉÍÓÖŐÚÜŰáéíóöőúüű";
    }
}
