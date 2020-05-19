using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI.Models
{
    public static class UserHandler
    {
        /// <summary>
        /// Method that gets all the users from the database
        /// </summary>
        /// <param name="db">db is the database to be passed to it of type ParknGardenData</param>
        /// <returns>Returns a dictionary where the key is the userId and the value is of type User for a specific value</returns>
        public static Dictionary<int, User> GetAllUsers(ParknGardenData db)
        {
            Dictionary<int, User> users = new Dictionary<int, User>();

            List<User> userDB = db.Users.ToList();

            foreach (User user in userDB)
            {
                users.Add(user.ID, user);
            }

            return users;

        }

        public static User GetOneUser(int userID, ParknGardenData db)
        {
            return db.Users.FirstOrDefault(u => u.ID == userID);
        }

        /// <summary>
        /// A method that creates a new user in the database
        /// </summary>
        /// <param name="db">db is the database to be passed to it of type ParknGardenData</param>
        /// <param name="user">user is the user to be added to the database</param>
        /// <returns>Returns the created user in the database so that it can be used elsewhere</returns>
        public static User PostUser(ParknGardenData db, User user)
        {
            bool userEmailInUse = db.Users.Any(u => u.Email == user.Email);

            if (!userEmailInUse)
            {
                User newUser = db.Users.Add(user);
                db.SaveChanges();
                return newUser;
            }

            user.ID = -1;
            return user;
        }

        /// <summary>
        /// A method that gets a dictionary of users depending on their UserLevel
        /// </summary>
        /// <param name="db">db is the database to be passed to it of type ParknGardenData</param>
        /// <param name="id">id is the userLevelId that is used to get the dictionary of users at a specific user level</param>
        /// <returns>Returns a dictionary where the key is the userId and the value is of type User for a specific value</returns>
        public static Dictionary<int, User> GetUsersByUserLevel(ParknGardenData db, int id)
        {
            Dictionary<int, User> users = new Dictionary<int, User>();

            List<User> userDB = db.Users.ToList();

            foreach (User user in userDB)
            {
                if(user.UserLevelID == id)
                    users.Add(user.ID, user);
            }

            return users;
        }

        /// <summary>
        /// A method that deletes a specified user from the database
        /// </summary>
        /// <param name="db">db is the database to be passed to it of type ParknGardenData</param>
        /// <param name="user">user is the user to be deleted from the database</param>
        public static void DeleteOneUser(ParknGardenData db, User user)
        {

            db.Users.Remove(user);
            db.SaveChanges();
        }
    }
}