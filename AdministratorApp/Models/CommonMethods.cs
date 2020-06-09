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
        /// <summary>
        /// Method that returns a role when given a dictionary of roles and a role ID
        /// </summary>
        /// <param name="roleId">The role ID that is used to find a role</param>
        /// <param name="roles">A dictionary in the format of the key being int and the value being of type Role</param>
        /// <returns>A role that corresponds to the given roleId</returns>
        public static Role GetRole(int roleId, Dictionary<int, Role> roles)
        {
            return roles[roleId];
        }

        public static User GetUser(int userId, Dictionary<int, User> users)
        {
            return users[userId];
        }

        /// <summary>
        /// Method that returns a salary when given a dictionary of salaries and a user ID, returns an empty salary if the salary was not found
        /// </summary>
        /// <param name="userId">Int type value that is the user ID used to find a salary</param>
        /// <param name="salaries">A dictionary that has the key of int and the value of salary</param>
        /// <returns>Either returns a salary that corresponds to the given userId if it finds one, otherwise creates a new salary with values 0, and 0 for before tax and tas percentage</returns>
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

        /// <summary>
        /// Method that returns a UserLevel given a userLevelID and a dictionary of user levels
        /// </summary>
        /// <param name="userLevelId">int, userLevelID is a value used to find a specific userLevel</param>
        /// <param name="userLevels">Dictionary with key int and value UserLevel</param>
        /// <returns>Returns the userLevel that corresponds to the given userLevelId</returns>
        public static UserLevel GetUserLevel(int userLevelId, Dictionary<int, UserLevel> userLevels)
        {
            return userLevels[userLevelId];
        }

        /// <summary>
        /// A method that returns a Store when given a storeId and a dictionary of stores
        /// </summary>
        /// <param name="storeId">int type value that is used to find a store</param>
        /// <param name="stores">a dictionary with key int and value Store</param>
        /// <returns>returns a store that corresponds to the given storeId</returns>
        public static Store GetStore(int storeId, Dictionary<int, Store> stores)
        {
            return stores[storeId];
        }

        /// <summary>
        /// Method for setting error text on delete, for trying to delete a user
        /// </summary>
        /// <param name="errors">type enum, pass an enum and get back a string for the error text depending on the enum</param>
        /// <returns>returns a string that contains error text depending on the passed enum</returns>
        public static string SetErrorTextOnDeleteForUsersPage(Constants.UserDeleteErorrs errors)
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
            if (categoryList.Count == 0)
            {
                foreach (Tuple<T, int> tuple in list)
                {
                    returnList.Add(tuple.Item1);
                }

                return returnList;
            }
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
            if (storeList.Count == 0)
            {
                foreach (Tuple<T, int> tuple in list)
                {
                    returnList.Add(tuple.Item1);
                }

                return returnList;
            }
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

        /// <summary>
        /// Filters a list specified by a list of stocks.
        /// </summary>
        /// <typeparam name="T">The object type to use and return.</typeparam>
        /// <param name="list">The list to filter (Contains a tuple of the T object, and list of int IDs of the stocks T object is part of.)</param>
        /// <param name="stockList">The list of stocks each item has to contain.</param>
        /// <returns>A list filtered by stocks.</returns>
        public static List<T> FilterListByStock<T>(List<Tuple<T, List<int>>> list, List<Stock> stockList)
        {
            List<T> returnList = new List<T>();
            if (stockList.Count == 0)
            {
                foreach (Tuple<T, List<int>> tuple in list)
                {
                    returnList.Add(tuple.Item1);
                }

                return returnList;
            }
            foreach (Tuple<T, List<int>> tuple in list)
            {
                foreach (Stock stock in stockList)
                {
                    foreach (int i in tuple.Item2)
                    {
                        if (stock.ID == i)
                        {
                            returnList.Add(tuple.Item1);
                            break;
                        }
                        
                    }

                    if (returnList.Contains(tuple.Item1))
                        break;
                }
            }

            return returnList;
        }

    }
}
