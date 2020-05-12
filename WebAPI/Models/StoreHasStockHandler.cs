using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI.Models
{
    public class StoreHasStockHandler
    {
        public static Dictionary<int, int> StoreHasStock(int storeID, ParknGardenData db)
        {
            Dictionary<int, int> itemsInStock = new Dictionary<int, int>();

            //List<StockHasItem> itemsDB = db.StockHasItems.Where(s => s.StockID == stockID).ToList();

          //  foreach (StockHasItem item in itemsDB)
            {
          //      itemsInStock.Add(item.ItemID, item.Amount);
            }

            return itemsInStock;
        }

        public static Dictionary<int, int> ItemsInStock(int itemID, ParknGardenData db)
        {
            Dictionary<int, int> itemsInStock = new Dictionary<int, int>();

            List<StockHasItem> itemsDB = db.StockHasItems.Where(s => s.ItemID == itemID).ToList();

            foreach (StockHasItem item in itemsDB)
            {
                itemsInStock.Add(item.StockID, item.Amount);
            }

            return itemsInStock;
        }

        public static Dictionary<int, Dictionary<int, int>> StockHasItems(ParknGardenData db)
        {
            Dictionary<int, Dictionary<int, int>> stockHasItems = new Dictionary<int, Dictionary<int, int>>();

            List<StockHasItem> itemsDB = db.StockHasItems.ToList();

            foreach (StockHasItem item in itemsDB)
            {
                if (!stockHasItems.ContainsKey(item.StockID))
                    stockHasItems.Add(item.StockID, new Dictionary<int, int>());
                stockHasItems[item.StockID].Add(item.ItemID, item.Amount);
            }

            return stockHasItems;

        }

        public static Dictionary<int, Dictionary<int, int>> ItemsInStock(ParknGardenData db)
        {
            Dictionary<int, Dictionary<int, int>> itemsInStock = new Dictionary<int, Dictionary<int, int>>();

            List<StockHasItem> itemsDB = db.StockHasItems.ToList();

            foreach (StockHasItem item in itemsDB)
            {
                if (!itemsInStock.ContainsKey(item.ItemID))
                    itemsInStock.Add(item.ItemID, new Dictionary<int, int>());
                itemsInStock[item.ItemID].Add(item.StockID, item.Amount);
            }

            return itemsInStock;

        }
    }
}