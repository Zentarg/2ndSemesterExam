using System;
using System.Collections.Generic;
using System.Text;

namespace CommonLibrary.Models
{
    public class Supplier
    {
        public Supplier() { }
        public Supplier(int id, string name, string address, string email, int phone)
        {
            Id = id;
            Name = name;
            Address = address;
            Email = email;
            Phone = phone;

        }

        //Constructor for post

        public Supplier(string name, string address, string email, int phone)
        {
            Name = name;
            Address = address;
            Email = email;
            Phone = phone;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public int Phone { get; set; }
        public override string ToString()
        {
            return $"#{Id}\t {Name}";
        }
    }
}
