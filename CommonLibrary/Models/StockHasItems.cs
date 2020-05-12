using System;
using System.Collections.Generic;
using System.Text;

namespace CommonLibrary.Models
{
   public class StockHasItems
    {
        public StockHasItems(int stockId, int itemId, int amount)
        {
            StockId = stockId;
            ItemId = itemId;
            Amount = amount;
        }

        public int StockId { get; set; }
        public int ItemId { get; set; }
        public int Amount { get; set; }
    }
}
