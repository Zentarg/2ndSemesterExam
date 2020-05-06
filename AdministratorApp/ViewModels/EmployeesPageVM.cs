using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using AdministratorApp.Models;
using CommonLibrary.Models;

namespace AdministratorApp.ViewModels
{
    class EmployeesPageVM : INotifyPropertyChanged
    {
        List<User> _emp = new List<User>();
        private User _sEmp =  new User();
        Dictionary<int, string> roles = new Dictionary<int, string>();
        Dictionary<int, Salary> _salaries = new Dictionary<int, Salary>();
        private Salary _objSalary;

        private string _name = "";
        private string _address = "";
        private int _telephone = 0;
        private string _role = "";
        private bool _IES = false;
        private string _selectedRole = "";
        private float _salary;
        private float _salaryWTax;
        private int _userId;

        public EmployeesPageVM()
        {
            _emp.Add(new User(1,"name1", "email1", 21673, "address1", 1, 25, 50, 200, 0, 1));
            _emp.Add(new User(2, "name2", "email2", 267318576, "address2", 2, 25, 50, 200, 0, 1));
            _emp.Add(new User(3,"name3", "email3", 612835, "address3", 3, 25, 50, 200, 0, 1));
            _emp.Add(new User(4,"name4", "email4", 827, "address4", 4, 25, 50, 200, 0, 1));
            _emp.Add(new User(5,"name5", "email5", 97123, "address5", 5, 25, 50, 200, 0, 1));
            roles.Add(1, "Owner");
            roles.Add(2, "Admin");
            roles.Add(3, "Manager");
            roles.Add(4, "Shelf Stocker");
            roles.Add(5, "Cash Register Worker");
            _salaries.Add(_emp[0].Id, new Salary(1, 2000, 50));
            _salaries.Add(_emp[1].Id, new Salary(2, 3000, 60));
            _salaries.Add(_emp[2].Id, new Salary(3, 2450, 53));
            _salaries.Add(_emp[3].Id, new Salary(4, 2283, 56));
            _salaries.Add(_emp[4].Id, new Salary(5, 7298, 48));



        }

        public List<User> Emp
        {
            get { return _emp; }
        }

        public User SelectedEmp
        {
            set { _sEmp = value;
                Name = _sEmp.Name;
                Telephone = _sEmp.Telephone;
                Address = _sEmp.Address;
                SelectedRole = null;
                _userId = _sEmp.Id;
                _objSalary = Models.CommonMethods.GetSalary(_userId, _salaries);
                Role = Models.CommonMethods.GetRole(_sEmp.RoleId, roles);
                Salary = _objSalary.BeforeTax;
                SalaryWTax = _objSalary.BeforeTax - (_objSalary.BeforeTax * (_objSalary.TaxPercentage / 100));
                IsEmployeeSelected = true;
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

        public string Role
        {
            get { return _role; }
            set { _role = value; OnPropertyChanged(); }
        }

        

        public List<string> Roles
        {
            get { return new List<string>(roles.Values); }

        }

        public bool IsEmployeeSelected
        {
            get { return _IES; }
            set { _IES = value; OnPropertyChanged(); }
        }

        public string SelectedRole
        {
            get { return _selectedRole; }
            set
            {
                Role = value;
                _selectedRole = value; OnPropertyChanged(); }
        }

        public float Salary
        {
            get { return _salary; }
            set { _salary = value; OnPropertyChanged(); }
        }

        public float SalaryWTax
        {
            get { return _salaryWTax; }
            set { _salaryWTax = value; OnPropertyChanged(); }
        }
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
