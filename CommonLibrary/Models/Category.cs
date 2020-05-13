using System;
using System.Collections.Generic;
using System.Text;

namespace CommonLibrary.Models
{
    public class Category
    {
        public Category() { }
        public Category( string name)
        {

            Name = name;
        }

        public Category(int id, string name)
        {
            ID = id;
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
