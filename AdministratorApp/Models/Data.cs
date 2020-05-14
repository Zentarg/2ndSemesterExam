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
        public static int OwnerID = 3;
        public static Dictionary<int, Item> AllItems { get; set; } = new Dictionary<int, Item>();
        public static Dictionary<int, Stock> AllStocks { get; set; } = new Dictionary<int, Stock>();
        public static Dictionary<int, Store> AllStores{ get; set; } = new Dictionary<int, Store>();
        public static Dictionary<int, User> AllUsers { get; set; } = new Dictionary<int, User>();
        public static Dictionary<int, Category> AllCategories { get; set; } = new Dictionary<int, Category>();
        public static Dictionary<int, Dictionary<int,int>> StockHasItems { get; set; } = new Dictionary<int, Dictionary<int, int>>();
        public static Dictionary<int, Dictionary<int, int>> ItemsInStocks { get; set; } = new Dictionary<int, Dictionary<int, int>>();
        public static Dictionary<int, Salary> AllSalaries { get; set; } = new Dictionary<int, Salary>();
        public static Dictionary<int, Role> AllRoles { get; set; } = new Dictionary<int, Role>();
        public static Dictionary<int, UserLevel> AllLevels { get; set; } = new Dictionary<int, UserLevel>();
        public static User SelectedUser { get; set; }
        public static User EditedUser { get; set; }
        public static Salary EditedSalary { get; set; }
        


        public static async Task UpdateItems()
        {
            AllItems = await APIHandler<Dictionary<int, Item>>.GetOne("Items");
        }

        public static async Task UpdateStock()
        {
            AllStocks = await APIHandler<Dictionary<int, Stock>>.GetOne("Stocks");
        }

        public static async Task UpdateStore()
        {
            AllStores = await APIHandler<Dictionary<int, Store>>.GetOne("Stores");
        }

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

        public static async Task UpdateSalaries()
        {
            AllSalaries = await APIHandler<Dictionary<int, Salary>>.GetOne("Salaries");
        }

        public static async Task UpdateRoles()
        {
            AllRoles = await APIHandler<Dictionary<int, Role>>.GetOne("Roles");
        }

        public static async Task<ObservableCollection<UserLevel>> UpdateUserLevels()
        {
            AllLevels = await APIHandler<Dictionary<int, UserLevel>>.GetOne("UserLevels");
            OwnerID = AllLevels.FirstOrDefault(l => l.Value.Name == "Owner").Value.Id;
            ObservableCollection<UserLevel> userLevels =  new ObservableCollection<UserLevel>();
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
    }


    
}
