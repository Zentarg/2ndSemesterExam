using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI.Models
{
    public static class StocksHandler
    {

        public static Dictionary<int, Stock> GetAllStocks(ParknGardenData db)
        {
            Dictionary<int, Stock> stocks = new Dictionary<int, Stock>();

            List<Stock> stockDB = db.Stocks.ToList();

            foreach (Stock stock in stockDB)
            {
                stocks.Add(stock.ID, stock);
            }

            return stocks;

        }
    }
}