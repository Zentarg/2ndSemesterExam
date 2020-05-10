using System;
using System.Collections.Generic;
using System.Text;

namespace CommonLibrary.Models
{
    public class Auth
    {
        public Auth()
        {

        }

        public Auth(string username, string passwordHash, string passwordSalt, int userId)
        {
            Username = username;
            PasswordHash = passwordHash;
            PasswordSalt = passwordSalt;
            UserID = userId;
        }
        public int UserID { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }

    }
}
