using System;
using AdministratorApp.Annotations;
using AdministratorApp.Models;
using CommonLibrary.Models;
using GalaSoft.MvvmLight.Command;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AdministratorApp.ViewModels
{
    public class AddStorePageVM : INotifyPropertyChanged
    {
        #region InstanceFields

        //Instance fields of object types
        private User _selectedManager;
        private Stock _selectedStock;
        Store _store = new Store();

        //Instance fields of primitive types
        private string _name = "";
        private string _address = "";
        private int _phone = 0;
        private int _storeId = 0;
        private string _errorText = "";

        #endregion

        /// <summary>
        /// Constructor for setting up commands, properties and retrieving data from Data.cs
        /// </summary>
        public AddStorePageVM()
        {
            LoadDataAsync();
            DoCreate = new RelayCommand(Create);
            DoCancel = new RelayCommand(Cancel);
        }

        #region Properties

        //Properties for relay commands
        public RelayCommand DoCreate { get; set; }
        public RelayCommand DoCancel { get; set; }

        //Observable Collection for retrieving all managers in the database using Data.cs
        public ObservableCollection<User> AllManagers
        {
            get
            {
                ObservableCollection<User> managers = new ObservableCollection<User>();
                foreach (User user in Data.AllUsers.Values)
                {
                    if (user.UserLevelId == 1)
                    {
                        managers.Add(user);
                    }
                }
                return managers;
            }
        }

        //Observable Collection for retrieving all stores in the database using Data.cs
        public ObservableCollection<Store> AllStores
        {
            get
            {
                return new ObservableCollection<Store>(Data.AllStores.Values);
            }
        }

        //Observable Collection for retrieving all stocks in the database using Data.cs
        public ObservableCollection<Stock> AllStocks
        {
            get { return new ObservableCollection<Stock>(Data.AllStocks.Values); }
        }

        //Object type properties
        public User SelectedManager
        {
            get { return _selectedManager; }
            set { _selectedManager = value; OnPropertyChanged(); }
        }

        public Stock SelectedStock
        {
            get { return _selectedStock; }
            set { _selectedStock = value; OnPropertyChanged(); }
        }

        //Primitive type properties
        public string Name
        {
            get { return _name; }
            set { _name = value; OnPropertyChanged(); }
        }

        public string Address
        {
            get { return _address; }
            set { _address = value; OnPropertyChanged(); }
        }

        public int Phone
        {
            get { return _phone; }
            set { _phone = value; OnPropertyChanged(); }
        }

        public string ErrorText
        {
            get { return _errorText; }
            set { _errorText = value; OnPropertyChanged(); }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Method for creating new stores and storing them in the database
        /// </summary>
        private async void Create()
        {
            if (CheckTextFields())
            { 
                _store = new Store(Name, Address, Phone, SelectedManager.Id, SelectedStock.ID);
                var item = AllStores.SingleOrDefault(s =>
                    s.Name == Name && s.Address == Address && s.Phone == Phone && s.ManagerID == SelectedManager.Id && s.StockId == SelectedStock.ID);

                if (item == null)
                {
                    ErrorText = "";
                    Store postedStore = await APIHandler<Store>.PostOne("Stores", _store);
                }

                else ErrorText = "Store already exists";
            }
        }

        /// <summary>
        /// Method that erases current text fields and navigates back to Store page
        /// </summary>
        private void Cancel()
        {
            Name = "";
            Address = "";
            Phone = 0;
            SelectedManager = null;
            SelectedStock = null;
            ErrorText = "";
            NavigationHandler.NavigateBackwards();
        }

        /// <summary>
        /// Method for checking the text field inputs to make sure they are not empty
        /// </summary>
        /// <returns>returns true when there is data in the fields, false when empty</returns>
        public bool CheckTextFields()
        {
            bool expression = !string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(Address) &&
                               !string.IsNullOrEmpty(Phone.ToString()) &&
                               !string.IsNullOrEmpty(SelectedManager.Id.ToString()) && !string.IsNullOrEmpty(SelectedStock.ID.ToString());
            if (expression)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Method for updating properties in Data.cs and loading some of the properties in StorePageVM.cs
        /// </summary>
        /// <returns></returns>
        private async Task LoadDataAsync()
        {
            await Data.UpdateUsers();
            await Data.UpdateStore();
            await Data.UpdateStock();

            
            OnPropertyChanged(nameof(AllManagers));
            OnPropertyChanged(nameof(AllStores));
            OnPropertyChanged(nameof(AllStocks));
        }

        #endregion

        #region PropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

    }
}
