using System;
using System.Collections.Generic;
using System.Text;

namespace CommonLibrary.Models
{
    class InvoiceStatus
    {
        public InvoiceStatus(int id, string name)
        {
            ID = id;
            Name = name;
        }

        public int ID { get; set; }
        public string Name { get; set; }
    }
}
