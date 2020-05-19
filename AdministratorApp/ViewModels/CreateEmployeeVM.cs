using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Mime;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Authentication.Web.Provider;
using Windows.Services.TargetedContent;
using AdministratorApp.Annotations;
using AdministratorApp.Models;
using AdministratorApp.Views;
using CommonLibrary.Models;
using GalaSoft.MvvmLight.Command;

namespace AdministratorApp.ViewModels
{
    public class CreateEmployeeVM : INotifyPropertyChanged
    {
        #region InstanceFields
        //Instance fields of type observable collection
        private ObservableCollection<Store> _stores = new ObservableCollection<Store>();
        private ObservableCollection<UserLevel> _accountTypes = new ObservableCollection<UserLevel>();
        private ObservableCollection<Role> _roles = new ObservableCollection<Role>();

        //Instance fields of object types
        private UserLevel _selectedUserLevel;
        private Store _selectedStore;
        private Role _selectedRole;
        private User _tempUser = new User();

        //Instance fields of more primitive types
        private string _name = "";
        private string _address = "";
        private int _telephone = 0;
        private float _salary;
        private float _salaryWTax;
        private int _tajNumber;
        private int _taxNumber;
        private float _workingHours;
        private string _userName;
        private string _email;
        private string _password = "********";
        private string _errorText = "";
        #endregion

        /// <summary>
        /// Constructor that sets up the needed methods and property and calls a method to update the relevant data in Data.cs
        /// </summary>
        public CreateEmployeeVM()
        {
            LoadDataAsync();
            DoConfirm = new RelayCommand(CreateUser);
            DoGenerateUserName = new RelayCommand(GenerateUserName);
            DoGeneratePassword = new RelayCommand(GeneratePassword);
            DoUpdateComboBoxRoles = new RelayCommand(UpdateRoleComboBox);
            VMHandler.CreateEmployeeVm = this;
        }

        #region Properties
        //Relay Command Properties
        public RelayCommand DoConfirm { get; set; }
        public RelayCommand DoGenerateUserName { get; set; }
        public RelayCommand DoGeneratePassword { get; set; }
        public RelayCommand DoUpdateComboBoxRoles { get; set; }

        //Observable Collection Properties
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
        public ObservableCollection<Store> Stores
        {
            get { return _stores; }
            set { _stores = value; OnPropertyChanged(); }
        }

        //Dictionary Properties
        public Dictionary<int, Store> DictStore
        {
            get { return Data.AllStores; }
        }

        //Object Type Properties
        public Role SelectedRole
        {
            get { return _selectedRole; }
            set { _selectedRole = value; OnPropertyChanged(); CheckTextFields(); }
        }
        public UserLevel SelectedUserLevel
        {
            get { return _selectedUserLevel; }
            set { _selectedUserLevel = value; OnPropertyChanged(); }
        }
        public Store SelectedStore
        {
            get { return _selectedStore; }
            set { _selectedStore = value; OnPropertyChanged(); }
        }

        //Primitive Type Properties
        public string Salt { get; set; }
        public string UserName
        {
            get { return _userName; }
            set { _userName = value; OnPropertyChanged(); }
        }
        public string Password
        {
            get { return _password; }
            set { _password = value; OnPropertyChanged(); }
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
        public string Email
        {
            get { return _email; }
            set { _email = value; OnPropertyChanged(); }
        }
        public string ErrorText
        {
            get { return _errorText; }
            set { _errorText = value; OnPropertyChanged(); }
        }
        #endregion


        
        /// <summary>
        /// Method that creates a new user in the system via the api by using the properties
        /// </summary>
        public async void CreateUser()
        {
            if (CheckTextFields())
            {
                if (!string.IsNullOrEmpty(UserName) && Password != "********")
                {
                    _tempUser = new User(Name, Email, Telephone, Address, SelectedRole.Id, TajNumber, TaxNumber, WorkingHours, SelectedUserLevel.Id, SelectedStore.ID);
                    User postedUser = await APIHandler<User>.PostOne("Users", _tempUser);
                    if (postedUser.Id != -1)
                    {
                        Salary salary = new Salary(postedUser.Id, Salary, (SalaryWTax / Salary) * 100);
                        Salary postedSalary = await APIHandler<Salary>.PostOne("Salaries", salary);
                        string encryptedPassword = AuthHandler.EncryptPassword(Password, Salt);
                        Auth posteAuth = await APIHandler<Auth>.PostOne("Auth/PostAuth", new Auth(UserName, encryptedPassword, Salt, postedUser.Id));
                        NavigationHandler.NavigateToPage(typeof(EmployeesPage));
                        VMHandler.EmployeesPageVm.FeedBackText = $"The user {postedUser.Name} \nwas created successfully";
                    }
                    else
                        ErrorText = "The provided email is in use,\nplease specify a different one.";
                }
                else 
                    ErrorText = "A password and username\nmust be generated before\nconfirming.";
            }
            else
                ErrorText = "All text fields must be filled\nand all selections must be\nmade.";

        }

        /// <summary>
        /// Method for updating the relevant properties in Data.cs and loading some of the properties in CreateEmployeeVM.cs
        /// </summary>
        /// <returns></returns>
        public async Task LoadDataAsync()
        {
            await Data.UpdateRoles();
            await Data.UpdateStore();
            AccountTypes = await Data.UpdateUserLevels();
            Roles = new ObservableCollection<Role>(Data.AllRoles.Values);

            foreach (Store s in DictStore.Values)
            {
                Stores.Add(s);
            }
        }

        /// <summary>
        /// Checks the text fields to make sure that they contain data within them
        /// </summary>
        /// <returns>returns a bool, true if all fields contain data and false if they dont</returns>
        public bool CheckTextFields()
        {
            bool expression = (!string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(Address) &&
                               !string.IsNullOrEmpty(Email) &&
                               Telephone != 0 && SelectedRole != null && SelectedUserLevel != null &&
                               SelectedStore != null &&
                               Salary != 0 && SalaryWTax != 0 && TajNumber != 0 && TaxNumber != 0 &&
                               WorkingHours != 0);
            if (expression)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Generates a random password and random salt for the new user
        /// </summary>
        public void GeneratePassword()
        {
            Password = AuthHandler.GenerateString(8);
            Salt = AuthHandler.GenerateString(16);
        }

        /// <summary>
        /// Generates a username for the user
        /// </summary>
        public async void GenerateUserName()
        {
            UserName = await AuthHandler.GenerateUserName(Name);
        }

        /// <summary>
        /// A method that is called to only update the roles, called when a new role is created
        /// </summary>
        public async void UpdateRoleComboBox()
        {
            await Data.UpdateRoles();
            OnPropertyChanged(nameof(Roles));
        }

        #region PropertyChange Event and Method
        public event PropertyChangedEventHandler PropertyChanged;
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

    }
}
