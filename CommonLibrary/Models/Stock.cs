using System;
using System.Collections.Generic;
using System.Text;

namespace CommonLibrary.Models
{
    public class Stock
    {
        public Stock(int stockId, string name)
        {
            ID = stockId;
            Name = name;
        }

        public int ID { get; set; }
        public string Name { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
