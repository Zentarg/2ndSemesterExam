using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonLibrary.Models;

namespace AdministratorApp.Models
{
    public static class RuntimeDataHandler
    {

        public static Stock SelectedStock
        {
            get => Data.AllStocks.Values.FirstOrDefault(x => x.StockID == RuntimeDataHandler.SelectedStore.StockId);
        }
        public static Store SelectedStore { get; set; }


    }
}
