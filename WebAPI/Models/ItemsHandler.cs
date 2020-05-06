using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI.Models
{
    public static class ItemsHandler
    {

        public static Dictionary<int, Item> GetAllItems(ParknGardenData db)
        {
            Dictionary<int, Item> items = new Dictionary<int, Item>();

            List<Item> itemsDB = db.Items.ToList();

            foreach (Item item in itemsDB)
            {
                items.Add(item.ID, item);
            }

            return items;

        }

    }
}