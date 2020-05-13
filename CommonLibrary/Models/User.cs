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
        //For GET requests
        public User(int id, string name, string email, int phone, string address, int roleId, int tajNumber,
            int taxNumber, float workingHours, int userLevelId, int storeId)
        {
            Id = id;
            Name = name;
            Email = email;
            Address = address;
            Phone = phone;
            RoleId = roleId;
            TAJNumber = tajNumber;
            TAXNumber = taxNumber;
            WorkingHours = workingHours;
            UserLevelId = userLevelId;
            StoreId = storeId;
        }
        //For POST requests with a new role
        public User(string name, string email, int phone, string address, int tajNumber,
            int taxNumber, float workingHours, int userLevelId, int storeId)
        {
            Name = name;
            Email = email;
            Address = address;
            Phone = phone;
            TAJNumber = tajNumber;
            TAXNumber = taxNumber;
            WorkingHours = workingHours;
            UserLevelId = userLevelId;
            StoreId = storeId;
        }
        //For POST requests with an existing role
        public User(string name, string email, int phone, string address, int roleId, int tajNumber,
            int taxNumber, float workingHours, int userLevelId, int storeId)
        {
            Name = name;
            Email = email;
            Address = address;
            Phone = phone;
            RoleId = roleId;
            TAJNumber = tajNumber;
            TAXNumber = taxNumber;
            WorkingHours = workingHours;
            UserLevelId = userLevelId;
            StoreId = storeId;
        }

        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Email { get; set; } = "";
        public int Phone { get; set; }
        public string Address { get; set; } = "";
        public int RoleId { get; set; }
        public int TAJNumber { get; set; }
        public int TAXNumber { get; set; }
        public float WorkingHours { get; set; }
        public int UserLevelId { get; set; }
        public int StoreId { get; set; }

        public override string ToString()
        {
            return $"{Name}";
        }
    }
}
