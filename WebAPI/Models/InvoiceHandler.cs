using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace WebAPI.Models
{
    public static class InvoiceHandler
    {
        public static Dictionary<int, Invoice> GetAllInvoices(ParknGardenData db)
        {
            Dictionary<int, Invoice> allInvoices = new Dictionary<int, Invoice>();

            foreach (Invoice invoice in db.Invoices)
            {
                allInvoices.Add(invoice.ID, invoice);
            }

            return allInvoices;

        }

        public static Dictionary<int, List<int>> GetAllInvoiceIDsFromStores(ParknGardenData db)
        {
            Dictionary<int, List<int>> allInvoices = new Dictionary<int, List<int>>();

            foreach (Invoice invoice in db.Invoices)
            {
                if (!allInvoices.ContainsKey(invoice.StoreID))
                    allInvoices.Add(invoice.StoreID, new List<int>());
                allInvoices[invoice.StoreID].Add(invoice.ID);
            }

            return allInvoices;

        }

        public static Dictionary<int, Dictionary<int, int>> GetAllInvoicesHasItems(ParknGardenData db)
        {
            Dictionary<int, Dictionary<int, int>> allInvoices = new Dictionary<int, Dictionary<int, int>>();

            foreach (InvoiceHasItem item in db.InvoiceHasItems)
            {
                if (!allInvoices.ContainsKey(item.InvoiceID))
                    allInvoices.Add(item.InvoiceID, new Dictionary<int, int>());
                allInvoices[item.InvoiceID].Add(item.ItemID, item.Amount);
            }

            return allInvoices;
        }


    }
}