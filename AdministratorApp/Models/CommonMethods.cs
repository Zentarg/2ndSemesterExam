using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.Streaming.Adaptive;
using CommonLibrary.Models;

namespace AdministratorApp.Models
{
    public static class CommonMethods
    {
        public static Role GetRole(int roleId, Dictionary<int, Role> roles)
        {
            return roles[roleId];
        }

        public static Salary GetSalary(int userId, Dictionary<int, Salary> salaries)
        {
            try
            {
                return salaries[userId];
            }
            catch (Exception e)
            {
                return new Salary(userId, 0, 0);
            }
        }

        public static UserLevel GetUserLevel(int userLevelId, Dictionary<int, UserLevel> userLevels)
        {
            return userLevels[userLevelId];
        }

        public static Store GetStore(int storeId, Dictionary<int, Store> stores)
        {
            return stores[storeId];
        }

        public static string SetErrorTextOnDelete(Constants.UserDeleteErorrs errors)
        {
            if (errors == Constants.UserDeleteErorrs.NO_SELECTED_USER)
                return "You must first select a user\nbefore trying to delete one";
            if (errors == Constants.UserDeleteErorrs.USER_LOGGED_IN)
                return "You cannot delete a logged\nin user";
            if (errors == Constants.UserDeleteErorrs.DELETE_OWNER)
                return "You cannot delete the owner\nin the system";
            if (errors == Constants.UserDeleteErorrs.LOW_ACCESS_LEVEL)
                return "You cannot delete users who\nhave the same or higher access level";
            if (errors == Constants.UserDeleteErorrs.DELETE_ID_0)
                return "You cannot delete the dummy\nuser from the system";
            return "";

        }

        /// <summary>
        /// Filters a list specified by string. The item object inside the list needs to have a .ToString() method corresponding to what the user is searching for.
        /// </summary>
        /// <typeparam name="T">The object type to use and return.</typeparam>
        /// <param name="list">The list to filter.</param>
        /// <param name="filterBy">The string each item has to contain.</param>
        /// <returns>A filtered list.</returns>
        public static List<T> FilterListByString<T>(List<T> list, string filterBy)
        {
            List<T> returnList = new List<T>();
            foreach (T item in list)
            {
                if (item.ToString().ToLower().Contains(filterBy.ToLower()))
                    returnList.Add(item);
            }

            return returnList;
        }

        /// <summary>
        /// Filters a list specified by string. The item object inside the list needs to have a .ToString() method corresponding to what the user is searching for.
        /// </summary>
        /// <typeparam name="T">The object type to use and return.</typeparam>
        /// <param name="list">The list of tuple containing item to filter, and string to add to said items ToString().</param>
        /// <param name="filterBy">The string each item has to contain.</param>
        /// <returns>A filtered list.</returns>
        public static List<T> FilterListByString<T>(List<Tuple<T, string>> list, string filterBy)
        {
            List<T> returnList = new List<T>();
            foreach (Tuple<T, string> item in list)
            {
                string stringToCheck = item.Item1 + " " + item.Item2;
                if (stringToCheck.ToLower().Contains(filterBy.ToLower()))
                    returnList.Add(item.Item1);
            }

            return returnList;
        }

        /// <summary>
        /// Filters a list specified by a list of categories.
        /// </summary>
        /// <typeparam name="T">The object type to use and return.</typeparam>
        /// <param name="list">The list to filter (Contains a tuple of the T object, and the int ID of the category.)</param>
        /// <param name="categoryList">The list of categories each item has to contain.</param>
        /// <returns>A list filtered by categories.</returns>
        public static List<T> FilterListByCategories<T>(List<Tuple<T, int>> list, List<Category> categoryList)
        {
            List<T> returnList = new List<T>();
            foreach (Tuple<T, int> tuple in list)
            {
                foreach (Category category in categoryList)
                {
                    if (category.ID == tuple.Item2)
                        returnList.Add(tuple.Item1);
                }

            }

            return returnList;
        }

        /// <summary>
        /// Filters a list specified by a list of stores.
        /// </summary>
        /// <typeparam name="T">The object type to use and return.</typeparam>
        /// <param name="list">The list to filter (Contains a tuple of the T object, and the int ID of the store.)</param>
        /// <param name="storeList">The list of stores each item has to contain.</param>
        /// <returns>A list filtered by stores.</returns>
        public static List<T> FilterListByStores<T>(List<Tuple<T, int>> list, List<Store> storeList)
        {
            List<T> returnList = new List<T>();
            foreach (Tuple<T, int> tuple in list)
            {
                foreach (Store store in storeList)
                {
                    if (store.ID == tuple.Item2)
                        returnList.Add(tuple.Item1);
                }
            }

            return returnList;
        }

    }
}
