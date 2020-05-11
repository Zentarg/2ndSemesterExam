using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdministratorApp.Models
{
    public static class RuntimeDataHandler
    {
        static RuntimeDataHandler()
        {
            SelectedStore = Data.AllStores[0];
        }


        public static  Store SelectedStore { get; set; }
    }
}
