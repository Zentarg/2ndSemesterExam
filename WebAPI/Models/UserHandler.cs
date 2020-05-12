using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI.Models
{
    public static class UserHandler
    {
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

        public static void DeleteOneUser(ParknGardenData db, User user)
        {

            db.Users.Remove(user);
            db.SaveChanges();
        }
    }
}