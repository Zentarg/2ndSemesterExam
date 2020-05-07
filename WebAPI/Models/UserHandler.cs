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

    }
}