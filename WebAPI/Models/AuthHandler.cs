using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace WebAPI.Models
{
    public static class AuthHandler
    {
        private static string connectionString = ConfigurationManager.ConnectionStrings["ParknGardenData"].ConnectionString;
        /// <summary>
        /// Gets password salt stored in database, identified by username.
        /// </summary>
        /// <param name="username">Account Username</param>
        /// <returns>Password Salt.</returns>
        public static string GetSalt(string username)
        {
            string query = "SELECT a.passwordSalt from Auth a WHERE a.Username = @username";

            string salt = null;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand command = new SqlCommand(query, conn);
                command.Parameters.AddWithValue("@username", username);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    salt = reader.GetString(0);
                }
            }

            return salt;
        }

        /// <summary>
        /// Logs in the user based on username and password. Creates a session key in database and returns it if the authentication succeeds
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="password">Password</param>
        /// <returns>Session key.</returns>
        public static string Login(string username, string password)
        {
            throw new NotImplementedException("Validating login not implemented yet.");
        }

        /// <summary>
        /// Checks if the session key is valid, and the user is logged in.
        /// </summary>
        /// <param name="sessionKey">Session key.</param>
        /// <returns>Boolean depicting whether or not the session key is valid.</returns>
        public static bool Authenticate(string sessionKey)
        {
            throw new NotImplementedException("Authenticating session key not implemented yet.");
        }

        /// <summary>
        /// Gets the user level of the user, identified by session key.
        /// </summary>
        /// <param name="sessionKey">Session key.</param>
        /// <returns>Returns int corresponding to UserLevel ID in the database.</returns>
        public static int GetUserLevel(string sessionKey)
        {
            throw new NotImplementedException("Getting user level not implemented yet.");
        }
    }
}