using AdministratorApp.Annotations;
using AdministratorApp.Models;
using CommonLibrary.Models;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using AdministratorApp.Views;

namespace AdministratorApp.ViewModels
{
    public class StorePageVM : INotifyPropertyChanged
    {
        #region InstanceFields

        //Instance fields of object types
        private Store _selectedStore = new Store();
        private User _selectedManager;
        private Stock _selectedStock;

        //Instance field for search bar
        private string _filterString = "";

        //Instance field for primitive types
        private string _name = "";
        private string _address = "";
        private int _phone = 0;
        private int _managerID = 0;
        private string _manager = "";
        private int _stockID = 0;
        private string _stock = "";
        private string _errorText = "";

        #endregion

        /// <summary>
        /// Constructor for setting up commands, properties and retrieving data from Data.cs
        /// </summary>
        public StorePageVM()
        {
            LoadDataAsync();

            DoConfirm = new RelayCommand(Confirm);
            DoDelete = new RelayCommand(Delete);
            DoCancel = new RelayCommand(Cancel);
            DoAddStore = new RelayCommand(AddStore);
     
        }

        #region Properties

        //Properties for Relay commands
        public RelayCommand DoConfirm { get; set; }
        public RelayCommand DoDelete { get; set; }
        public RelayCommand DoCancel { get; set; }
        public RelayCommand DoAddStore { get; set; }

        //Property for search filtering strings in the search bar
        public string FilterString
        {
            get => _filterString;
            set
            {
                _filterString = value;
                OnPropertyChanged(nameof(FilteredStores));
            }
        }

        //Property for using the method in CommonMethods.cs for the search bar
        public ObservableCollection<Store> FilteredStores
        {
            get
            {
                ObservableCollection<Store> stores = new ObservableCollection<Store>();

                foreach (Store store in CommonMethods.FilterListByString(Data.AllStores.Values.ToList(), FilterString))
                {
                    if (store.ID != 0)
                        stores.Add(store);
                }

                return stores;
            }

        }

        //Observable Collection for retrieving all stores in the database using Data.cs
        public ObservableCollection<Store> StoreList
        {
            get
            {
                ObservableCollection<Store> stores = new ObservableCollection<Store>(Data.AllStores.Values);

                if (stores.Count > 0)
                {
                    if (stores[0].ID == 0)
                    {
                        stores.RemoveAt(0);
                    }
                }

                return stores;
            }
        }

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

        //Observable Collection for retrieving all stocks in the database using Data.cs
        public ObservableCollection<Stock> AllStocks
        {
            get { return new ObservableCollection<Stock>(Data.AllStocks.Values);}
        }

        //Object type properties
        public Store SelectedStore
        {
            get
            {
                return _selectedStore;
            }
            set
            {
                _selectedStore = value;
                if (_selectedStore != null)
                {
                    Name = _selectedStore.Name;
                    Address = _selectedStore.Address;
                    Phone = _selectedStore.Phone;
                    ManagerID = Data.AllStores[SelectedStore.ID].ManagerID;
                    Manager = Data.AllUsers[ManagerID].Name;
                    StockID = Data.AllStores[SelectedStore.ID].StockId;
                    Stock = Data.AllStocks[StockID].Name;
                }
                OnPropertyChanged();
                OnPropertyChanged(nameof(StoreIsSelected));
            }
        }

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

        public string Manager
        {
            get { return _manager; }
            set { _manager = value; OnPropertyChanged(); }
        }

        public int ManagerID
        {
            get { return _managerID; }
            set { _managerID = value; OnPropertyChanged(); }
        }

        public string Stock
        {
            get { return _stock; }
            set { _stock = value; OnPropertyChanged(); }
        }

        public int StockID
        {
            get { return _stockID; }
            set { _stockID = value; OnPropertyChanged(); }
        }

        public string ErrorText
        {
            get { return _errorText; }
            set { _errorText = value; OnPropertyChanged(); }
        }

        public bool IsVisible
        {
            get => AuthHandler.ShowAdministratorFunctions;
        }

        public bool StoreIsSelected
        {
            get
            {
                if (SelectedStore.ID == 0)
                {
                    return false;
                }

                return true;
            } 
        }

        #endregion

        #region Methods

        /// <summary>
        /// Method that edits the current selected store and stores it in the database
        /// </summary>
        private async void Confirm()
        {
            if (CheckTextFields())
            {
                Data.AllStores[SelectedStore.ID].Name = Name;
                Data.AllStores[SelectedStore.ID].Address = Address;
                Data.AllStores[SelectedStore.ID].Phone = Phone;
                Data.AllStores[SelectedStore.ID].ManagerID = SelectedManager.Id;
                Data.AllStores[SelectedStore.ID].StockId = SelectedStock.ID;

                await APIHandler<Store>.PutOne("Stores/PutStore/" + SelectedStore.ID, Data.AllStores[SelectedStore.ID]);

                SelectedStore = null;
                ErrorText = "Information has been changed.";
                OnPropertyChanged(nameof(StoreList));
            }
            else 
                ErrorText = "All text fields must be filled\nand all selections must be made.";
        }

        /// <summary>
        /// Method that deletes current selected store and removes it from the database
        /// </summary>
        private async void Delete()
        {
            if (SelectedStore != null)
            {
                await APIHandler<Store>.DeleteOne("Stores/DeleteStore/" + SelectedStore.ID);
                Data.AllStores.Remove(SelectedStore.ID);
                OnPropertyChanged(nameof(StoreList));
                Cancel();
            }
        }

        /// <summary>
        /// Method that erases current text fields
        /// </summary>
        private void Cancel()
        {
            Name = "";
            Address = "";
            Phone = 0;
            SelectedManager = null;
            SelectedStock = null;
            ErrorText = "";
        }

        private void AddStore()
        {
            NavigationHandler.NavigateToPage(typeof(AddStorePage));
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

            OnPropertyChanged(nameof(FilteredStores));
            OnPropertyChanged(nameof(AllManagers));
            OnPropertyChanged(nameof(AllStocks));
        }

        /// <summary>
        /// Method for checking the text field inputs to make sure they are not empty
        /// </summary>
        /// <returns>returns true when there is data in the fields, false when empty</returns>
        public bool CheckTextFields()
        {
            bool expression = (!string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(Address) &&
                               !string.IsNullOrEmpty(Phone.ToString()) && SelectedManager != null && SelectedStock != null);
            if (expression)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// ToString method for listing store's name, address and phone
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{Name} {Address} {Phone}";
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
