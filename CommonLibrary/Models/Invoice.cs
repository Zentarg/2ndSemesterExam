using System;
using System.Collections.Generic;
using System.Text;

namespace CommonLibrary.Models
{
    public class Invoice
    {
        public Invoice(int id, int authorId, float price, float discount, string comment, int storeId, int invoiceStatusId, string adminComment)
        {
            ID = id;
            AuthorID = authorId;
            Price = price;
            Discount = discount;
            Comment = comment;
            StoreID = storeId;
            InvoiceStatusID = invoiceStatusId;
            AdminComment = adminComment;
        }

        public int ID { get; set; }
        public int AuthorID { get; set; }
        public float Price { get; set; }
        public float Discount { get; set; }
        public string Comment { get; set; }
        public int StoreID { get; set; }
        public int InvoiceStatusID { get; set; }
        public string AdminComment { get; set; }

        public override string ToString()
        {
            return $"#{ID}";
        }
    }
}
