using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI.Models
{
    public static class StoresHandler
    {

        public static Dictionary<int, Store> GetAllStores(ParknGardenData db)
        {
            Dictionary<int, Store> stores = new Dictionary<int, Store>();

            List<Store> storeDB = db.Stores.ToList();

            foreach (Store store in storeDB)
            {
                stores.Add(store.ID, store);
            }

            return stores;

        }

    }
}