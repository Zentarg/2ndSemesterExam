using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using CommonLibrary.Models;

namespace AdministratorApp.ViewModels
{
    class EmployeesPageVM : INotifyPropertyChanged
    {
        List<Employee> _emp = new List<Employee>();
        Employee _sEmp =  new Employee();

        private string _name = "";
        private string _address = "";
        private int _telephone = 0;

        public EmployeesPageVM()
        {
            _emp.Add(new Employee("name1", "email1", 21673, "address1", 1, 25, 50, 200, 0, 1));
            _emp.Add(new Employee("name2", "email2", 267318576, "address2", 2, 25, 50, 200, 0, 1));
            _emp.Add(new Employee("name3", "email3", 612835, "address3", 3, 25, 50, 200, 0, 1));
            _emp.Add(new Employee("name4", "email4", 827, "address4", 4, 25, 50, 200, 0, 1));
            _emp.Add(new Employee("name5", "email5", 97123, "address5", 5, 25, 50, 200, 0, 1));
        }

        public List<Employee> Emp
        {
            get { return _emp; }
        }

        public Employee SelectedEmp
        {
            set { _sEmp = value;
                Name = _sEmp.Name;
                Telephone = _sEmp.Telephone;
                Address = _sEmp.Address;
                OnPropertyChanged();}
            get { return _sEmp; }
        }

        public string Name
        {
            set { _name = value; OnPropertyChanged(); }
            get { return _name; }
        }
        public string Address
        {
            set { _address = value; OnPropertyChanged(); }
            get { return _address; }
        }

        public int Telephone
        {
            set { _telephone = value; OnPropertyChanged();}
            get { return _telephone; }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
