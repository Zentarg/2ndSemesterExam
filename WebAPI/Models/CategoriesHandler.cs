using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI.Models
{
    public class CategoriesHandler
    {

        public static Dictionary<int, Category> GetAllCategories(ParknGardenData db)
        {
            Dictionary<int, Category> categories = new Dictionary<int, Category>();

            List<Category> categoriesDB = db.Categories.ToList();

            foreach (Category c in categoriesDB)
            {
                categories.Add(c.ID, c);
            }

            return categories;

        }
    }
}