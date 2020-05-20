using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI.Models
{
    public class SupplierHandler
    {
        /// <summary>
        /// Method to get all the Suppliers from the database to a dictionary, where the key is the Supplier ID and the Value is the Supplier. It has the database sa a paramater
        /// </summary>
        /// <param name="db"> </param>
        /// <returns></returns>
        public static Dictionary<int, Supplier> GetAllSupplier(ParknGardenData db)
        {
            Dictionary<int, Supplier> suppliers = new Dictionary<int, Supplier>();

            List<Supplier> supplierDB = db.Suppliers.ToList();

            foreach (Supplier supplier in supplierDB)
            {
                suppliers.Add(supplier.ID, supplier);
            }

            return suppliers;

        }
    }
}