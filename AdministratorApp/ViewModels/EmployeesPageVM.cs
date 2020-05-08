using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using AdministratorApp.Models;
using CommonLibrary.Models;
using GalaSoft.MvvmLight.Command;

namespace AdministratorApp.ViewModels
{
    class EmployeesPageVM : INotifyPropertyChanged
    {
        ObservableCollection<User> _users = new ObservableCollection<User>();
        private User _sEmp =  new User();
        private ObservableCollection<string> _stores = new ObservableCollection<string>();
        private ObservableCollection<string> _roles = new ObservableCollection<string>();
        private Salary _objSalary;

        private string _name = "";
        private string _address = "";
        private int _telephone = 0;
        private string _role = "";
        private bool _IES = false;
        private string _selectedRole = "";
        private float _salary;
        private float _salaryWTax;
        private int _userId = -1;
        private float _tajNumber;
        private float _taxNumber;
        private float _workingHours;
        private string _selectedStore;
        private string _userName;
        private string _email;
        

        public EmployeesPageVM()
        {
            LoadDataAsync();
            DoShowUserName = new RelayCommand(GetUserName);
        }

        public Dictionary<int, User> DictUsers
        {
            get { return Data.AllUsers; }
            set { Data.AllUsers = value; 
                OnPropertyChanged(); }
        }

        public RelayCommand DoShowUserName { get; set; }
        public string UserName
        {
            get { return _userName; }
            set
            {
                _userName = value; OnPropertyChanged(); }
        }
        public Dictionary<int, Salary> DictSalaries
        {
            get { return Data.AllSalaries; }
            set { Data.AllSalaries = value; OnPropertyChanged(); }
        }

        public Dictionary<int, Role> DictRoles
        {
            get { return Data.AllRoles; }
            set { Data.AllRoles = value; OnPropertyChanged(); }
        }

        public Dictionary<int, Store> DictStore
        {
            get { return Data.AllStores; }
        }

        public ObservableCollection<User> Users
        {
            get
            {
                ObservableCollection<User> users = new ObservableCollection<User>(Data.AllUsers.Values);
                return users;
            }
            set
            {
                _users = value; OnPropertyChanged();
            }
        }

        public User SelectedEmp
        {
            set { _sEmp = value;
                Name = _sEmp.Name;
                Telephone = _sEmp.Phone;
                Address = _sEmp.Address;
                SelectedRole = null;
                _userId = _sEmp.Id;
                _objSalary = CommonMethods.GetSalary(_userId, DictSalaries);
                Role = CommonMethods.GetRole(_sEmp.RoleId, DictRoles).Name;
                Salary = _objSalary.BeforeTax;
                SalaryWTax = _objSalary.BeforeTax - (_objSalary.BeforeTax * (_objSalary.TaxPercentage / 100));
                IsEmployeeSelected = true;
                TajNumber = _sEmp.TAJNumber;
                TaxNumber = _sEmp.TAXNumber;
                WorkingHours = _sEmp.WorkingHours;
                SelectedStore = DictStore[_sEmp.StoreId].Name;
                Email = _sEmp.Email;
                UserName = "";
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

        public ObservableCollection<string> Stores
        {
            get { return _stores; }
            set { _stores = value; OnPropertyChanged(); }
        }

        public ObservableCollection<string> Roles
        {
            get { return _roles; }
            set { _roles = value; OnPropertyChanged(); }

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

        public float TajNumber
        {
            get { return _tajNumber; }
            set { _tajNumber = value; OnPropertyChanged();}
        }

        public float TaxNumber
        {
            get { return _taxNumber; }
            set { _taxNumber = value; OnPropertyChanged();}
        }

        public float WorkingHours
        {
            get { return _workingHours; }
            set { _workingHours = value; OnPropertyChanged();}
        }

        public string SelectedStore
        {
            get { return _selectedStore; }
            set { _selectedStore = value; OnPropertyChanged(); }
        }

        public string Email
        {
            get { return _email; }
            set { _email = value; OnPropertyChanged(); }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private async Task LoadDataAsync()
        {
            await Data.UpdateUsers();
            await Data.UpdateSalaries();
            await Data.UpdateRoles();
            await Data.UpdateStore();
            foreach (Role r in DictRoles.Values)
            {
                Roles.Add(r.Name);
            }

            foreach (Store s in DictStore.Values)
            {
                Stores.Add(s.Name);
            }
            OnPropertyChanged(nameof(Users));
            OnPropertyChanged(nameof(DictUsers));
        }

        private async void GetUserName()
        {
            if (_userId != -1)
            {
                UserName = await APIHandler<string>.GetOne($"auth/getusername/{_userId}");
            }
        }
    }
}
