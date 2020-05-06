using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI.Models
{
    public static class StockHasItemsHandler
    {
        public static Dictionary<int,int> StockHasItems(int stockID, ParknGardenData db)
        {
            Dictionary<int,int> itemsInStock = new Dictionary<int, int>();

            List<StockHasItem> itemsDB = db.StockHasItems.Where(s => s.StockID == stockID).ToList();

            foreach (StockHasItem item in itemsDB)
            {
                itemsInStock.Add(item.ItemID, item.Amount);
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
    }
}