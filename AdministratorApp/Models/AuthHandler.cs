using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using CommonLibrary.Models;

namespace AdministratorApp.Models
{
    public static class AuthHandler
    {

        public static int UserID { get; set; }
        public static string SessionKey { get; set; }
        public static User ActiveUser { get; set; }
        public static string ActiveUserRoleName { get; set; }
        public static Constants.AccessLevels ActiveUserAccessLevel { get; set; }
        public static string ActiveUserLevelName { get; set; }
        public static Store ActiveUserStore { get; set; }

        public static bool ShowAdministratorFunctions =>
            ActiveUserAccessLevel == Constants.AccessLevels.Administrator ||
            ActiveUserAccessLevel == Constants.AccessLevels.Owner;

        public static string EncryptPassword(string input, string salt)
        {

            MD5 md = MD5.Create();

            byte[] inputBytes = System.Text.Encoding.UTF8.GetBytes(input);
            byte[] hashBytes = md.ComputeHash(inputBytes);

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("X"));
            }

            inputBytes = System.Text.Encoding.UTF8.GetBytes(sb.ToString() + salt);
            hashBytes = md.ComputeHash(inputBytes);
            sb = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("X"));
            }

            return sb.ToString();
        }

        public static async Task InitializeAuth()
        {
            ActiveUser = await APIHandler<User>.GetOne($"Users/{UserID}");
            ActiveUserRoleName = (await APIHandler<Role>.GetOne($"Roles/{ActiveUser.RoleId}")).Name;
            ActiveUserAccessLevel = (Constants.AccessLevels) ActiveUser.UserLevelId;
            ActiveUserLevelName = (await APIHandler<UserLevel>.GetOne($"UserLevels/{ActiveUser.UserLevelId}")).Name;
            ActiveUserStore = await APIHandler<Store>.GetOne($"Stores/{ActiveUser.StoreId}");

        }

        public static async Task Logout()
        {
            await APIHandler<Session>.DeleteOne($"Auth/DeleteSession/{SessionKey}");
        }


    }
}
