using System;
using System.Collections.Generic;
using System.Text;

namespace CommonLibrary.Models
{
    public class InvoiceHasItem
    {
        public InvoiceHasItem(int invoiceId, int itemId, int amount)
        {
            InvoiceID = invoiceId;
            ItemID = itemId;
            Amount = amount;
        }

        public int InvoiceID { get; set; }
        public int ItemID { get; set; }
        public int Amount { get; set; }
    }
}
