using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Core.Mapping;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace WebAPI.Models
{
    public static class AuthHandler
    {
        private static Random rd = new Random();
        private static int sessionKeyLength = 32;
        /// <summary>
        /// Gets password salt stored in database, identified by username.
        /// </summary>
        /// <param name="username">Account Username.</param>
        /// <param name="db">DBContext to pull from.</param>
        /// <returns>Password Salt.</returns>
        public static string GetSalt(string username, ParknGardenData db)
        {
            return db.Auths.FirstOrDefault(u => u.Username == username)?.PasswordSalt;
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
        /// Deletes session from db, with the sesseionKey specified.
        /// </summary>
        /// <param name="sessionKey">SessionKey of the session to be deleted.</param>
        /// <param name="db">DBContext to use.</param>
        /// <returns>The session that was deleted.</returns>
        public static Session DeleteSession(string sessionKey, ParknGardenData db)
        {
            Session session = db.Sessions.FirstOrDefault(s => s.SessionKey == sessionKey);
            if (session != null)
            {
                db.Sessions.Remove(session);
                db.SaveChanges();
            }
            return session;
        }

        /// <summary>
        /// Logs in the user based on username and password. Creates a session key in database and returns it if the authentication succeeds
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="password">Password</param>
        /// <param name="db">DBContext to pull from.</param>
        /// <returns>Session key.</returns>
        public static (int UserID, string SessionKey) Login(string username, string password, ParknGardenData db)
        {
            string sessionKey = null;
            int userID = -1;

            string passwordHash = db.Auths.FirstOrDefault(u => u.Username == username)?.PasswordHash;
            userID = db.Auths.FirstOrDefault(u => u.Username == username)?.UserID ?? -1;
            if (password == passwordHash && userID != -1)
            {
                sessionKey = CreateSessionKey(sessionKeyLength);

                while (Authenticate(sessionKey, db))
                {
                    sessionKey = CreateSessionKey(sessionKeyLength);
                }

                db.Sessions.Add(new Session(){SessionKey = sessionKey, UserID = userID});
                db.SaveChanges();
            }
                
            return (UserID: userID, SessionKey: sessionKey);

        }

        /// <summary>
        /// Checks if the session key is valid, and the user is logged in.
        /// </summary>
        /// <param name="sessionKey">Session key.</param>
        /// <param name="db">DBContext to pull from.</param>
        /// <returns>Boolean depicting whether or not the session key is valid.</returns>
        public static bool Authenticate(string sessionKey, ParknGardenData db)
        {
            return db.Sessions.Any(s => s.SessionKey == sessionKey);
        }

        /// <summary>
        /// Gets the user level of the user, identified by session key.
        /// </summary>
        /// <param name="sessionKey">Session key.</param>
        /// <returns>Returns int corresponding to UserLevel ID in the database.</returns>
        public static int GetUserLevel(string sessionKey, ParknGardenData db)
        {
            if (!Authenticate(sessionKey, db))
                return -1;

            return db.UserLevels.FirstOrDefault(
                l => l.ID == db.Users.FirstOrDefault(
                        u => u.ID == db.Sessions.FirstOrDefault(
                            s => s.SessionKey == sessionKey).UserID).UserLevelID)?.ID ?? -1;
        }

        /// <summary>
        /// Method that checks if the logged in user has the correct session key
        /// </summary>
        /// <param name="sessionKey">The passed session key which is used to check for existing in db</param>
        /// <param name="userId">The passed userId which is used to check for existing in db</param>
        /// <param name="db">The database</param>
        /// <returns>returns enum type with different errors which can be used to determine if a user can be verified</returns>
        public static Constants.VerifyUserErrors VerifyUserSession(string sessionKey, int userId, ParknGardenData db)
        {
            Session currentSession = db.Sessions.FirstOrDefault(s => s.SessionKey == sessionKey);

            if (!Authenticate(sessionKey, db))
                return Constants.VerifyUserErrors.SESSION_NOT_FOUND;

            if (currentSession != null)
            {
                if (currentSession.UserID == userId)
                    return Constants.VerifyUserErrors.OK;
                if (currentSession.UserID != userId)
                    return Constants.VerifyUserErrors.INCORRECT_SESSION_KEY;
            }

            return Constants.VerifyUserErrors.SESSION_NOT_FOUND;
        }

        /// <summary>
        /// A method for getting the username for a specific user's userId
        /// </summary>
        /// <param name="userID">userID is an int that is used to find the username</param>
        /// <param name="db">db is the database to be passed to it of type ParknGardenData</param>
        /// <returns>returns the username of type string, if it can find one for the specified userId</returns>
        public static string GetUserName(int userID, ParknGardenData db)
        {
            var user = db.Auths.FirstOrDefault(u => u.UserID == userID);
            return user?.Username;
        }

        /// <summary>
        /// A method that checks if a username is in use in the database
        /// </summary>
        /// <param name="userName">userName is the string that is used to see if it exists in the database</param>
        /// <param name="db">db is the database to be passed to it of type ParknGardenData</param>
        /// <returns>returns true if the username is in use and false if not</returns>
        public static bool CheckUserName(string userName, ParknGardenData db)
        {
            if (db.Auths.FirstOrDefault(u => u.Username == userName) == null)
                return false;
            return true;
        }

        /// <summary>
        /// A method that adds a new auth to the database
        /// </summary>
        /// <param name="auth">auth is the auth to be added to the database</param>
        /// <param name="db">db is the database to be passed to it of type ParknGardenData</param>
        public static void PostNewAuth(Auth auth, ParknGardenData db)
        {
            db.Auths.Add(auth);
        }

        /// <summary>
        /// A method that deletes a given auth from the database
        /// </summary>
        /// <param name="db">db is the database to be passed to it of type ParknGardenData</param>
        /// <param name="auth">auth is the given auth that is to be deleted from the database</param>
        public static void DeleteUserAuth(ParknGardenData db, Auth auth)
        {
            db.Auths.Remove(auth);
            db.SaveChanges();
        }
    }
}