using System;
using System.Collections.Generic;
using System.Text;

namespace CommonLibrary.Models
{
    public class Stock
    {
        public Stock(int stockId, string name)
        {
            StockID = stockId;
            Name = name;
        }

        public int StockID { get; set; }
        public string Name { get; set; }
    }
}
