using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonLibrary.Models;

namespace AdministratorApp.Models
{
    public static class Data
    {
        public static List<Item> AllItems { get; set; }
        public static List<Stock> AllStocks { get; set; }

        public static async Task UpdateItems()
        {
            AllItems = await APIHandler<Item>.GetMultiple("Items");
            AllStocks = await APIHandler<Stock>.GetMultiple("Stocks");
        }

    }
}
