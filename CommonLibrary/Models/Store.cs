using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdministratorApp.Models
{
    public class Store
    {
        

        public Store(int id, int administratorId, int managerId, string name, string address, int telephone, string manager)
        {
            ID = id;
            AdministratorID = administratorId;
            ManagerID = managerId;
            Name = name;
            Address = address;
            Telephone = telephone;
            Manager = manager;
        }


        public int ID { get; set; }
        public int AdministratorID { get; set; }
        public int ManagerID { get; set; }
        public string Manager { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public int Telephone { get; set; }
 
        

    }
}
