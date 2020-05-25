using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
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

        /// <summary>
        /// Double encrypts password based on input string and input salt.
        /// </summary>
        /// <param name="input">Rawtext password.</param>
        /// <param name="salt">Rawtext salt.</param>
        /// <returns>Double encrypted password.</returns>
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

        /// <summary>
        /// Initializes all the properties that might be needed throughout the program lifecycle.
        /// </summary>
        /// <returns>Task, enables await.</returns>
        public static async Task InitializeAuth()
        {
            ActiveUser = await APIHandler<User>.GetOne($"Users/{UserID}");
            ActiveUserRoleName = (await APIHandler<Role>.GetOne($"Roles/{ActiveUser.RoleId}")).Name;
            ActiveUserAccessLevel = (Constants.AccessLevels) ActiveUser.UserLevelId;
            ActiveUserLevelName = (await APIHandler<UserLevel>.GetOne($"UserLevels/{ActiveUser.UserLevelId}")).Name;
            ActiveUserStore = await APIHandler<Store>.GetOne($"Stores/{ActiveUser.StoreId}");
        }


        /// <summary>
        /// Deletes session key from db.
        /// </summary>
        /// <returns>Task, enables await.</returns>
        public static async Task Logout()
        {
            await APIHandler<Session>.DeleteOne($"Auth/DeleteSession/{SessionKey}");
        }

        /// <summary>
        /// Method that generates a username for a user using their name
        /// </summary>
        /// <param name="Name">Type string, Name is the string of characters that will be used to create a username</param>
        /// <returns>returns a Task of type string that contains the generated username</returns>
        public static async Task<string> GenerateUserName(string Name)
        {
            string userNameWTag = "";
            if (!string.IsNullOrEmpty(Name))
            {
                string[] strings = Name.Split("");
                string userNameBase = "";
                if (strings[0].Length > 4)
                {
                    userNameBase = strings[0].Substring(0, 4);
                }
                else
                {
                    userNameBase = strings[0];
                }

                userNameWTag = userNameBase + GenerateTag();
            }

            if (await APIHandler<bool>.GetOne($"Auth/GetUserNameExists/{userNameWTag}"))
                await GenerateUserName(Name);
            string UserName = userNameWTag.ToLower();
            return UserName;
        }

        /// <summary>
        /// A method that generates a 4 digit combination of digits
        /// </summary>
        /// <returns>Returns a string of random 4 digits</returns>
        public static string GenerateTag()
        {
            Random numberGenerator = new Random();
            int randomInt = numberGenerator.Next(0, 10000);
            string identifier = randomInt.ToString();

            for (int i = 4; i > identifier.Length; i = i)
            {
                identifier = "0" + identifier;
            }

            return identifier;
        }

        /// <summary>
        /// Generates a random string
        /// </summary>
        /// <param name="length">int, how long the string will be</param>
        /// <returns>A random string of specified length</returns>
        public static string GenerateString(int length)
        {
            string returnString = RandomStringGenerator(length);
            return returnString;
        }

        /// <summary>
        /// A generator for generating a random string
        /// </summary>
        /// <param name="length">length is how long the string will be</param>
        /// <returns>returns a string of the specified length filled with random alphanumeric characters (upper and lower case)</returns>
        public static string RandomStringGenerator(int length)
        {
            Random randomString = new Random();
            const string allowedChars = "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz0123456789";

            char[] chars = new char[length];

            for (int i = 0; i < length; i++)
            {
                chars[i] = allowedChars[randomString.Next(0, allowedChars.Length)];
            }
            return new string(chars);
        }
    }
}
