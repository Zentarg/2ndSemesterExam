using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Perception.Provider;
using Windows.UI.Xaml;
using AdministratorApp.Models;
using AdministratorApp.Views;
using CommonLibrary.Models;
using GalaSoft.MvvmLight.Command;

namespace AdministratorApp.ViewModels
{
    public class EmployeesPageVM : INotifyPropertyChanged
    {
        private ObservableCollection<UserLevel> _accountTypes = new ObservableCollection<UserLevel>();
        private ObservableCollection<Store> _stores = new ObservableCollection<Store>();
        private ObservableCollection<Role> _roles = new ObservableCollection<Role>();
        private Salary _objSalary;
        private UserLevel _initialUserLevel;
        private UserLevel _selectedUserLevel;
        private Role _selectedRole;
        private Role _role;
        private Store _selectedStore;
        private Store _initialStore;
        private User _sEmp = new User();

        private string _name = "";
        private string _address = "";
        private int _telephone = 0;
        
        private bool _IES = false;

        private float _salary;
        private float _salaryWTax;
        private int _userId = -1;
        private int _tajNumber;
        private int _taxNumber;
        private float _workingHours;

        private string _userName;
        private string _email;
        private bool _showEdit = false;
        private bool _showNormal = true;
        private bool _showUserLevelEdit = false;
        private bool _showUserLevelNormal = true;
        

        private string _feedbackText = "";

        public EmployeesPageVM()
        {
            LoadDataAsync();
            DoShowUserName = new RelayCommand(GetUserName);
            DoCancel = new RelayCommand(Cancel);
            DoDelete = new RelayCommand(DeleteUser);
            DoShowEdit = new RelayCommand(ShowEditMethod);
            DoCancelEdit = new RelayCommand(CancelEditMethod);
            DoConfirmEdit = new RelayCommand(ConfirmEditMethod);
            VMHandler.EmployeesPageVm = this;
        }

        public Dictionary<int, User> DictUsers
        {
            get { return Data.AllUsers; }
            set { Data.AllUsers = value; 
                OnPropertyChanged(); }
        }

        public RelayCommand DoShowUserName { get; set; }
        public RelayCommand DoCancel { get; set; }
        public RelayCommand DoDelete { get; set; }
        public RelayCommand DoShowEdit { get; set; }
        public RelayCommand DoCancelEdit { get; set; }
        public RelayCommand DoConfirmEdit { get; set; }
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
        }

        public User SelectedEmp
        {
            set
            {
                User temp = _sEmp;
                _sEmp = value;
                if (_sEmp != null)
                {
                    if (ShowEdit == false)
                    {
                        if (ShowEdit)
                            if (temp != _sEmp)
                                SelectedEmp = temp;
                        Name = _sEmp.Name;
                        Telephone = _sEmp.Phone;
                        Address = _sEmp.Address;
                        SelectedRole = null;
                        _userId = _sEmp.Id;
                        _objSalary = CommonMethods.GetSalary(_userId, DictSalaries);
                        InitialRole = CommonMethods.GetRole(_sEmp.RoleId, Data.AllRoles);
                        InitialUserLevel = CommonMethods.GetUserLevel(_sEmp.UserLevelId, Data.AllLevels);

                        Salary = _objSalary.BeforeTax;
                        SalaryWTax = _objSalary.BeforeTax - (_objSalary.BeforeTax * (_objSalary.TaxPercentage / 100));
                        IsEmployeeSelected = true;
                        TajNumber = _sEmp.TAJNumber;
                        TaxNumber = _sEmp.TAXNumber;
                        WorkingHours = _sEmp.WorkingHours;
                        InitialStore = DictStore[_sEmp.StoreId];
                        Email = _sEmp.Email;
                        UserName = "";
                        FeedBackText = "";
                    }
                    
                }
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

        public Role InitialRole
        {
            get { return _role; }
            set { _role = value; OnPropertyChanged(); }
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

        public bool IsEmployeeSelected
        {
            get { return _IES; }
            set { _IES = value; OnPropertyChanged(); }
        }

        public Role SelectedRole
        {
            get { return _selectedRole; }
            set
            { _selectedRole = value; OnPropertyChanged(); }
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
            set { _tajNumber = value; OnPropertyChanged();}
        }

        public int TaxNumber
        {
            get { return _taxNumber; }
            set { _taxNumber = value; OnPropertyChanged();}
        }

        public float WorkingHours
        {
            get { return _workingHours; }
            set { _workingHours = value; OnPropertyChanged();}
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

        public string FeedBackText
        {
            get { return _feedbackText; }
            set { _feedbackText = value; OnPropertyChanged(); }
        }

        public bool ShowEdit
        {
            get { return _showEdit; }
            set { _showEdit = value; OnPropertyChanged(); }
        }

        public bool ShowNormal
        {
            get { return _showNormal; }
            set { _showNormal = value; OnPropertyChanged(); }
        }

        public bool ShowUserLevelEdit
        {
            get { return _showUserLevelEdit; }
            set { _showUserLevelEdit = value; OnPropertyChanged(); }
        }

        public bool ShowUserLevelNormal
        {
            get { return _showUserLevelNormal; }
            set { _showUserLevelNormal = value; OnPropertyChanged(); }
        }

        public UserLevel InitialUserLevel
        {
            get { return _initialUserLevel; }
            set { _initialUserLevel = value; OnPropertyChanged(); }
        }

        public UserLevel SelectedUserLevel
        {
            get { return _selectedUserLevel; }
            set { _selectedUserLevel = value; OnPropertyChanged(); }
        }

        public Store InitialStore
        {
            get { return _initialStore; }
            set { _initialStore = value; OnPropertyChanged(); }
        }

        public ObservableCollection<UserLevel> AccountTypes
        {
            get { return _accountTypes; }
            set { _accountTypes = value; OnPropertyChanged(); }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public async Task LoadDataAsync()
        {
            await Data.UpdateUsers();
            await Data.UpdateSalaries();
            await Data.UpdateRoles();
            await Data.UpdateStore();
            Roles = new ObservableCollection<Role>(Data.AllRoles.Values);
            AccountTypes = await Data.UpdateUserLevels();
            foreach (Store s in DictStore.Values)
            {
                Stores.Add(s);
            }
            OnPropertyChanged(nameof(Users));
            OnPropertyChanged(nameof(DictUsers));
        }

        public async Task LoadRolesAsync()
        {
            await Data.UpdateRoles();
            Roles = new ObservableCollection<Role>(Data.AllRoles.Values);
        }

        private async void GetUserName()
        {
            if (_userId != -1)
            {
                UserName = await APIHandler<string>.GetOne($"auth/getusername/{_userId}");
            }
        }

        public void Cancel()
        {
            Name = "";
            Telephone = 0;
            Address = "";
            SelectedRole = null;
            _userId = 0;
            _objSalary = null;
            InitialRole = null;
            Salary = 0;
            SalaryWTax = 0;
            IsEmployeeSelected = false;
            TajNumber = 0;
            TaxNumber = 0;
            WorkingHours = 0;
            SelectedStore = null;
            Email = "";
            UserName = "";
            SelectedEmp = null;
            SelectedStore = null;
            FeedBackText = "";
        }

        public async void DeleteUser()
        {
            Data.SelectedUser = SelectedEmp;
            VMHandler.EmployeesPageVm = this;
            DeleteUserConfirmationContentDialog deleteUserConfirmationContentDialog = new DeleteUserConfirmationContentDialog();
            await deleteUserConfirmationContentDialog.ShowAsync();
        }

        public void ShowEditMethod()
        {
            if (SelectedEmp.UserLevelId < AuthHandler.ActiveUser.UserLevelId)
            {
                ShowEdit = true;
                ShowUserLevelEdit = true;
                ShowNormal = false;
                ShowUserLevelNormal = false;
                SelectedStore = InitialStore;
                SelectedRole = InitialRole;
                SelectedUserLevel = InitialUserLevel;
                FeedBackText = "";
            }

            if (SelectedEmp.Id == AuthHandler.ActiveUser.Id)
            {
                ShowEdit = true;
                ShowUserLevelEdit = false;
                ShowUserLevelNormal = true;
                SelectedUserLevel = InitialUserLevel;
                ShowNormal = false;
                SelectedStore = InitialStore;
                SelectedRole = InitialRole;
                FeedBackText = "";
            }
            if (SelectedEmp.UserLevelId >= AuthHandler.ActiveUser.UserLevelId && SelectedEmp.Id != AuthHandler.ActiveUser.Id)
                FeedBackText = "You cannot edit data for\nusers with the same or\nhigher access level";
        }

        public void CancelEditMethod()
        {
            ShowEdit = false;
            ShowNormal = true;
            ShowUserLevelNormal = true;
            ShowUserLevelEdit = false;
            FeedBackText = "";
        }

        public async void ConfirmEditMethod()
        {
            FeedBackText = "";
            Data.SelectedUser = SelectedEmp;
            Data.EditedUser = new User(SelectedEmp.Id, Name, Email, Telephone, Address, SelectedRole.Id, TajNumber, TaxNumber, WorkingHours, SelectedUserLevel.Id, SelectedStore.ID);
            Data.EditedSalary = new Salary(SelectedEmp.Id, Salary, (SalaryWTax / Salary) * 100);
            VMHandler.EmployeesPageVm = this;
            ConfrimEditUserContentDialog cEUCD = new ConfrimEditUserContentDialog();
            await cEUCD.ShowAsync();
        }
    }
}
