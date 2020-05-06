using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace CommonLibrary.Models
{
    public class User
    {
        public User()
        {

        }

        public User(int id, string name, string email, int telephone, string address, int roleId, int tajNumber,
            int taxNumber, float workingHours, int userLevelId, int storeId)
        {
            Id = id;
            Name = name;
            Email = email;
            Address = address;
            Telephone = telephone;
            RoleId = roleId;
            TajNumber = tajNumber;
            TaxNumber = taxNumber;
            WorkingHours = workingHours;
            UserLevelId = userLevelId;
            StoreId = storeId;
        }
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Email { get; set; } = "";
        public int Telephone { get; set; }
        public string Address { get; set; } = "";
        public int RoleId { get; set; }
        public int TajNumber { get; set; }
        public int TaxNumber { get; set; }
        public float WorkingHours { get; set; }
        public int UserLevelId { get; set; }
        public int StoreId { get; set; }
    }
}
