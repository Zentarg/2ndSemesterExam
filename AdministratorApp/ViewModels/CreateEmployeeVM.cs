using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Mime;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.Services.TargetedContent;
using AdministratorApp.Annotations;
using AdministratorApp.Models;
using AdministratorApp.Views;
using CommonLibrary.Models;
using GalaSoft.MvvmLight.Command;

namespace AdministratorApp.ViewModels
{
    class CreateEmployeeVM : INotifyPropertyChanged
    {
        private User _tempUser = new User();
        private ObservableCollection<Store> _stores = new ObservableCollection<Store>();
        private ObservableCollection<UserLevel> _accountTypes = new ObservableCollection<UserLevel>();

        private UserLevel _selectedUserLevel;
        private Store _selectedStore;

        private string _name = "";
        private string _address = "";
        private int _telephone = 0;
        private Role _selectedRole;
        private float _salary;
        private float _salaryWTax;
        private int _tajNumber;
        private int _taxNumber;
        private float _workingHours;
        private string _userName;
        private string _email;
        private string _password = "********";
        private string _errorText = "";

        public CreateEmployeeVM()
        {
            LoadDataAsync();
            DoConfirm = new RelayCommand(CreateUser);
            DoGenerateUserName = new RelayCommand(GenerateUserName);
            DoGeneratePassword = new RelayCommand(GeneratePassword);
        }

        public RelayCommand DoConfirm { get; set; }
        public RelayCommand DoGenerateUserName { get; set; }
        public RelayCommand DoGeneratePassword { get; set; }
        public string Salt { get; set; }

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

        public ObservableCollection<Store> Stores
        {
            get { return _stores; }
            set { _stores = value; OnPropertyChanged(); }
        }

        public ObservableCollection<Role> Roles => Data.RolesList;

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
                _selectedRole = value;
                OnPropertyChanged();
                CheckTextFields();
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

        public string ErrorText
        {
            get { return _errorText; }
            set { _errorText = value; OnPropertyChanged();}
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

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


        public async void LoadDataAsync()
        {
            await Data.UpdateRoles();
            await Data.UpdateStore();
            AccountTypes = await Data.UpdateUserLevels();
            

            foreach (Store s in DictStore.Values)
            {
                Stores.Add(s);
            }
        }

        public async void GenerateUserName()
        {
            string userNameWTag = "";
            if (!string.IsNullOrEmpty(Name))
            { 
                string[] strings = Name.Split("");
                string userNameBase = "";
                if (strings[0].Length > 4)
                {
                    userNameBase = strings[0].Substring(0, 4);
                }
                else
                {
                    userNameBase = strings[0];
                }

                userNameWTag = userNameBase + GenerateTag();
            }
            
            if (await APIHandler<bool>.GetOne($"Auth/GetUserNameExists/{userNameWTag}"))
                GenerateUserName();
            UserName = userNameWTag.ToLower();
        }

        public string GenerateTag()
        {
            Random numberGenerator = new Random();
            int randomInt = numberGenerator.Next(0, 10000);
            string identifier = randomInt.ToString();

            for (int i = 4; i > identifier.Length; i = i)
            {
                identifier = "0" + identifier;
            }

            return identifier;
        }

        public void GeneratePassword()
        {
            Password = RandomStringGenerator(8);
            Salt = RandomStringGenerator(16);
        }

        public string RandomStringGenerator(int length)
        {
            Random randomString = new Random();
            const string allowedChars = "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz0123456789";

            char[] chars = new char[length];

            for (int i = 0; i < length; i++)
            {
                chars[i] = allowedChars[randomString.Next(0, allowedChars.Length)];
            }
            return new string(chars);
        }

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
    }
}
