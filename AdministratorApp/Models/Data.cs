using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.Streaming.Adaptive;
using CommonLibrary.Models;

namespace AdministratorApp.Models
{
    public static class Data
    {
        public static Dictionary<int, Item> AllItems { get; set; } = new Dictionary<int, Item>();
        public static Dictionary<int, Stock> AllStocks { get; set; } = new Dictionary<int, Stock>();
        public static Dictionary<int, Store> AllStores{ get; set; } = new Dictionary<int, Store>();
        public static Dictionary<int, User> AllUsers { get; set; } = new Dictionary<int, User>();
        public static Dictionary<int, Dictionary<int,int>> StockHasItems { get; set; } = new Dictionary<int, Dictionary<int, int>>();
        public static Dictionary<int, Dictionary<int, int>> ItemsInStocks { get; set; } = new Dictionary<int, Dictionary<int, int>>();



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

    }


    
}
