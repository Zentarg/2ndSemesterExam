using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonLibrary.Models;

namespace AdministratorApp.Models
{
    public class Store
    {
        public Store()
        {

        }

        //For creating new stores
        public Store(string name, string address, int phone, int managerID, int stockId)
        {
            Name = name;
            Address = address;
            Phone = phone;
            ManagerID = managerID;
            StockId = stockId;
        }

        public Store(int id, int administratorId, int managerId, string name, string address, int phone, string manager, int stockId)
        {
            ID = id;
            ManagerID = managerId;
            Name = name;
            Address = address;
            Phone = phone;
            Manager = manager;
            StockId = stockId;
        }


        public int ID { get; set; }
        public int ManagerID { get; set; }
        public string Manager { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public int Phone { get; set; }
        public int StockId { get; set; }

        public override string ToString()
        {
            return Name;
        } 
    }
}
