using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace WebAPI.Models
{
    public static class AuthHandler
    {
        private static string connectionString = ConfigurationManager.ConnectionStrings["ParknGardenData"].ConnectionString;
        private static Random rd = new Random();
        private static int sessionKeyLength = 32;
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
        /// Creates a random string with the length specified.
        /// </summary>
        /// <param name="length">Length of the string</param>
        /// <returns>String of random characters.</returns>
        public static string CreateSessionKey(int length)
        {

            const string allowedChars = "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz0123456789";

            char[] chars = new char[length];

            for (int i = 0; i < length; i++)
            {
                chars[i] = allowedChars[rd.Next(0, allowedChars.Length)];
            }

            return new string(chars);

        }

        /// <summary>
        /// Logs in the user based on username and password. Creates a session key in database and returns it if the authentication succeeds
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="password">Password</param>
        /// <returns>Session key.</returns>
        public static string Login(string username, string password)
        {

            string query = "SELECT a.userID, a.passwordHash from Auth a WHERE a.Username = @username";
            string sessionQuery = "INSERT INTO Session (SessionKey, UserID) VALUES (@sessionKey, @userID)";

            string sessionKey = null;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                int userID = -1;
                string passwordHash = "";
                conn.Open();
                SqlCommand command = new SqlCommand(query, conn);
                command.Parameters.AddWithValue("@username", username);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    userID = reader.GetInt32(0);
                    passwordHash = reader.GetString(1);
                }

                if (password == passwordHash && userID != -1)
                {
                    sessionKey = CreateSessionKey(sessionKeyLength);
                    while (Authenticate(sessionKey))
                    {
                        sessionKey = CreateSessionKey(sessionKeyLength);
                    }

                    SqlCommand sessionCommand = new SqlCommand(sessionQuery, conn);
                    sessionCommand.Parameters.AddWithValue("@sessionKey", sessionKey);
                    sessionCommand.Parameters.AddWithValue("@userID", userID);
                    int affectedRows = sessionCommand.ExecuteNonQuery();
                }
            }
            return sessionKey;

        }

        /// <summary>
        /// Checks if the session key is valid, and the user is logged in.
        /// </summary>
        /// <param name="sessionKey">Session key.</param>
        /// <returns>Boolean depicting whether or not the session key is valid.</returns>
        public static bool Authenticate(string sessionKey)
        {
            string query = "SELECT * FROM Session WHERE SessionKey = @sessionKey";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand command = new SqlCommand(query, conn);
                command.Parameters.AddWithValue("@sessionKey", sessionKey);
                SqlDataReader reader = command.ExecuteReader();
                return reader.HasRows;
            }
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