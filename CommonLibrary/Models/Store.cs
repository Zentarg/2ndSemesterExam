using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdministratorApp.Models
{
    public class Store
    {
        public Store()
        {
            
        }

        //For creating new stores
        public Store(string name, string address, int phone, string manager)
        {
            Name = name;
            Address = address;
            Phone = phone;
            Manager = manager;
        }

        public Store(int id, int administratorId, int managerId, string name, string address, int phone, string manager)
        {
            ID = id;
            AdministratorID = administratorId;
            ManagerID = managerId;
            Name = name;
            Address = address;
            Phone = phone;
            Manager = manager;
        }


        public int ID { get; set; }
        public int AdministratorID { get; set; }
        public int ManagerID { get; set; }
        public string Manager { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public int Phone { get; set; }

    }
}
