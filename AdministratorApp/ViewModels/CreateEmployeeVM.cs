using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.Services.TargetedContent;
using AdministratorApp.Annotations;
using AdministratorApp.Models;
using CommonLibrary.Models;
using GalaSoft.MvvmLight.Command;

namespace AdministratorApp.ViewModels
{
    class CreateEmployeeVM : INotifyPropertyChanged
    {
        private User _tempUser = new User();
        private ObservableCollection<Store> _stores = new ObservableCollection<Store>();
        private ObservableCollection<Role> _roles = new ObservableCollection<Role>();
        private ObservableCollection<UserLevel> _accountTypes = new ObservableCollection<UserLevel>();

        private Salary _objSalary;
        private UserLevel _selectedUserLevel;
        private Store _selectedStore;

        private string _name = "";
        private string _address = "";
        private int _telephone = 0;
        private string _role = "";
        private Role _selectedRole;
        private float _salary;
        private float _salaryWTax;
        private int _tajNumber;
        private int _taxNumber;
        private float _workingHours;
        private string _userName;
        private string _email;

        private int _userId = -1;

        public CreateEmployeeVM()
        {
            LoadDataAsync();
        }

        public RelayCommand DoConfirm { get; set; }

        public Dictionary<int, Role> DictRoles
        {
            get { return Data.AllRoles; }
            set { Data.AllRoles = value; OnPropertyChanged(); }
        }

        public Dictionary<int, Store> DictStore
        {
            get { return Data.AllStores; }
        }

        public string UserName
        {
            get { return _userName; }
            set
            {
                _userName = value; OnPropertyChanged();
            }
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
            set { _telephone = value; OnPropertyChanged(); }
            get { return _telephone; }
        }

        public string Role
        {
            get { return _role; }
            set { _role = value;
                SelectedRole = null; OnPropertyChanged(); }
        }

        public ObservableCollection<Store> Stores
        {
            get { return _stores; }
            set { _stores = value; OnPropertyChanged(); }
        }

        public ObservableCollection<Role> Roles
        {
            get { return _roles; }
            set { _roles = value; OnPropertyChanged(); }

        }

        public ObservableCollection<UserLevel> AccountTypes
        {
            get { return _accountTypes; }
            set { _accountTypes = value; OnPropertyChanged(); }

        }

        public Role SelectedRole
        {
            get { return _selectedRole; }
            set
            {
                
                _selectedRole = value; Role = ""; OnPropertyChanged();
            }
        }

        public UserLevel SelectedUserLevel
        {
            get { return _selectedUserLevel; }
            set { _selectedUserLevel = value; OnPropertyChanged(); }
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

        public int TajNumber
        {
            get { return _tajNumber; }
            set { _tajNumber = value; OnPropertyChanged(); }
        }

        public int TaxNumber
        {
            get { return _taxNumber; }
            set { _taxNumber = value; OnPropertyChanged(); }
        }

        public float WorkingHours
        {
            get { return _workingHours; }
            set { _workingHours = value; OnPropertyChanged(); }
        }

        public Store SelectedStore
        {
            get { return _selectedStore; }
            set { _selectedStore = value; OnPropertyChanged(); }
        }

        public string Email
        {
            get { return _email; }
            set { _email = value; OnPropertyChanged(); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void CreateUser()
        {
            if (SelectedRole == null)
            {
                Role newRole = new Role(0, Role);

                _tempUser = new User(Name, Email, Telephone, Address, TajNumber, TaxNumber, TaxNumber, SelectedUserLevel.Id, SelectedStore.ID);
                
            }
            else
            {
                _tempUser = new User(Name, Email, Telephone, Address, SelectedRole.Id, TajNumber,  TaxNumber, TaxNumber, SelectedUserLevel.Id, SelectedStore.ID);
            }

        }

        public async void LoadDataAsync()
        {
            await Data.UpdateRoles();
            await Data.UpdateStore();
            AccountTypes = await Data.UpdateUserLevels();
            foreach (Role r in DictRoles.Values)
            {
                Roles.Add(r);
            }

            foreach (Store s in DictStore.Values)
            {
                Stores.Add(s);
            }

            
        }
    }
}
