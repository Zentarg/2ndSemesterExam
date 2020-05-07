using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI.Models
{
    public class UserLevelHandler
    {

        public static Dictionary<int, UserLevel> GetAllUserLevels(ParknGardenData db)
        {
            Dictionary<int, UserLevel> userLevels = new Dictionary<int, UserLevel>();

            List<UserLevel> userLevelDB = db.UserLevels.ToList();

            foreach (UserLevel userLevel in userLevelDB)
            {
                userLevels.Add(userLevel.ID, userLevel);
            }

            return userLevels;

        }

        public static UserLevel GetOneUserLevel(int userLevelID, ParknGardenData db)
        {
            return db.UserLevels.FirstOrDefault(u => u.ID == userLevelID);
        }

    }
}