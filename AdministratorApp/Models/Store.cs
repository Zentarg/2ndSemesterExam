using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdministratorApp.Models
{
    public class Store
    {
        

        public Store(string name, string address, int telephone, string manager)
        {
            Name = name;
            Address = address;
            Telephone = telephone;
            Manager = manager;
        }


        public List<Store> StoreList
        { 
            get { return new List<Store>()
                {
                    new Store("John Doe's", "Some place", 84758439, "John Doe"),
                    new Store("Your Mom", "Your Mom's", 123456789, "Your Dad's")
                };
            }
        }

        public string Name { get; set; }
        public string Address { get; set; }
        public int Telephone { get; set; }
        public string Manager { get; set; }

        public override string ToString()
        {
            return $"{Name} {Address} {Telephone} {Manager}";
        }
    }
}
