using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Store.Preview.InstallControl;
using Windows.Media.Streaming.Adaptive;
using CommonLibrary.Models;

namespace AdministratorApp.Models
{
    public static class Data
    {
        /// <summary>
        /// The ID given at runtime for the Owner access level
        /// </summary>
        public static int OwnerID = 3;

        public static Dictionary<int, Item> AllItems { get; set; } = new Dictionary<int, Item>();
        public static Dictionary<int, Stock> AllStocks { get; set; } = new Dictionary<int, Stock>();
        public static Dictionary<int, Store> AllStores{ get; set; } = new Dictionary<int, Store>();

        /// <summary>
        /// A dictionary of users where the key is the userId and the value is of type User
        /// </summary>
        public static Dictionary<int, User> AllUsers { get; set; } = new Dictionary<int, User>();
        public static Dictionary<int, Category> AllCategories { get; set; } = new Dictionary<int, Category>();
        public static Dictionary<int, Dictionary<int,int>> StockHasItems { get; set; } = new Dictionary<int, Dictionary<int, int>>();
        public static Dictionary<int, Dictionary<int, int>> ItemsInStocks { get; set; } = new Dictionary<int, Dictionary<int, int>>();

        /// <summary>
        /// A dictionary of salaries where the key is the userId and the value is of type Salary
        /// </summary>
        public static Dictionary<int, Salary> AllSalaries { get; set; } = new Dictionary<int, Salary>();

        /// <summary>
        /// A dictionary of roles where the key is the roleId and the value is of type Role
        /// </summary>
        public static Dictionary<int, Role> AllRoles { get; set; } = new Dictionary<int, Role>();

        /// <summary>
        /// A dictionary of User Levels where the key is the userLevelId and the value is of type UserLevel
        /// </summary>
        public static Dictionary<int, UserLevel> AllLevels { get; set; } = new Dictionary<int, UserLevel>();
        public static Dictionary<int, Invoice> AllInvoices { get; set; } = new Dictionary<int, Invoice>();
        public static Dictionary<int, Dictionary<int, int>> InvoiceHasItems { get; set; } = new Dictionary<int, Dictionary<int, int>>();
        public static Dictionary<int, Supplier> AllSuppliers { get; set; } = new Dictionary<int, Supplier>();
        /// <summary>
        /// A User that is the selected user in the users page
        /// </summary>
        public static User SelectedUser { get; set; }
        /// <summary>
        /// A user that has been edited for the selected user from the users page
        /// </summary>
        public static User EditedUser { get; set; }
        /// <summary>
        /// A salary that has been edited for the selected user from the users page
        /// </summary>
        public static Salary EditedSalary { get; set; }
        /// <summary>
        /// A supplier that is the selected supplier in the suppliers page
        /// </summary>
        /// <returns></returns>
        public static Supplier SelectedSupplier { get; set; }

        /// <summary>
        /// A supplier that has been edited and will be the put supplier
        /// </summary>
        /// <returns></returns>
        public static Supplier EditedSupplier { get; set; }
        public static List<Log> AllLogs { get; set; }


        public static async Task UpdateItems()
        {
            AllItems = await APIHandler<Dictionary<int, Item>>.GetOne("Items");
        }

        public static async Task UpdateStock()
        {
            AllStocks = await APIHandler<Dictionary<int, Stock>>.GetOne("Stocks");
        }

        /// <summary>
        /// A method used to update the dictionary of stores using the API
        /// </summary>
        /// <returns></returns>
        public static async Task UpdateStore()
        {
            AllStores = await APIHandler<Dictionary<int, Store>>.GetOne("Stores");
        }

        /// <summary>
        /// A method used to update the dictionary of users using the API
        /// </summary>
        /// <returns></returns>
        public static async Task UpdateUsers()
        {
            AllUsers = await APIHandler<Dictionary<int, User>>.GetOne("Users");
        }

        public static async Task UpdateStockHasItems()
        {
            StockHasItems = await APIHandler<Dictionary<int, Dictionary<int, int>>>.GetOne("StockHasItems");
        }

        public static async Task UpdateItemsInStocks()
        {
            ItemsInStocks = await APIHandler<Dictionary<int, Dictionary<int, int>>>.GetOne("ItemsInStocks");
        }

        public static async Task UpdateCategories()
        {
            AllCategories = await APIHandler<Dictionary<int, Category>>.GetOne("Categories");
        }

        /// <summary>
        /// A method used to update the dictionary of salaries using the API
        /// </summary>
        /// <returns></returns>
        public static async Task UpdateSalaries()
        {
            AllSalaries = await APIHandler<Dictionary<int, Salary>>.GetOne("Salaries");
        }

        /// <summary>
        /// A method used to update the dictionary of roles using the API
        /// </summary>
        /// <returns></returns>
        public static async Task UpdateRoles()
        {
            AllRoles = await APIHandler<Dictionary<int, Role>>.GetOne("Roles");
        }

        /// <summary>
        /// A method that updates the list of userLevels, used for editing and creating new users and assigning them a user level using the API
        /// </summary>
        /// <returns>A Task of type ObservableCollection of userLevels that is from the lowest level up to but not including the level of the logged in user</returns>
        public static async Task<ObservableCollection<UserLevel>> UpdateUserLevels()
        {
            AllLevels = await APIHandler<Dictionary<int, UserLevel>>.GetOne("UserLevels");
            OwnerID = AllLevels.FirstOrDefault(l => l.Value.Name == "Owner").Value.Id;
            ObservableCollection<UserLevel> userLevels = new ObservableCollection<UserLevel>();
            foreach (UserLevel uL in Data.AllLevels.Values)
            {
                if (AuthHandler.ActiveUser.UserLevelId != uL.Id)
                {
                    userLevels.Add(uL);
                }
                else
                {
                    break;
                }
            }

            return userLevels;
        }

        public static async Task UpdateInvoices()
        {
            AllInvoices = await APIHandler<Dictionary<int, Invoice>>.GetOne("Invoices");
        }

        public static async Task UpdateInvoiceHasItems()
        {
            InvoiceHasItems = await APIHandler<Dictionary<int, Dictionary<int, int>>>.GetOne("InvoicesHasItems");
        }

        public static async Task UpdateSuppliers()
        {
            AllSuppliers = await APIHandler<Dictionary<int, Supplier>>.GetOne("Suppliers");
        }

        public static async Task UpdateLogs()
        {
            AllLogs = await APIHandler<List<Log>>.GetOne($"Logs/{AuthHandler.ActiveUser.Id}/{AuthHandler.SessionKey}");
        }
    }
}


    

