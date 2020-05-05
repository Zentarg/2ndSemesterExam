using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AdministratorApp.Models
{
    public static class AuthHandler
    {

        public static int UserID { get; set; }
        public static string SessionKey { get; set; }

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



    }
}
