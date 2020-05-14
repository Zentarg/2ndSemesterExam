using AdministratorApp.Annotations;
using AdministratorApp.Models;
using CommonLibrary.Models;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace AdministratorApp.ViewModels
{
    public class StorePageVM : INotifyPropertyChanged
    {
        private Store _store = new Store();
        private Store _selectedStore = new Store();
        private ObservableCollection<Store> _allStores = new ObservableCollection<Store>();
        private ObservableCollection<string> _managers = new ObservableCollection<string>();
        private User _selectedManager;

        private bool _isEditing = false;
        private string _name = "";
        private string _address = "";
        private int _phone = 0;
        private int _managerID = 0;
        private string _manager = "";

        public StorePageVM()
        {
            LoadDataAsync();

            GoToStockPage = new RelayCommand(StockPage);
            DoConfirm = new RelayCommand(Confirm);
            DoDelete = new RelayCommand(Delete);
            DoCancel = new RelayCommand(Cancel);

        }

        public RelayCommand GoToStockPage { get; set; }
        public RelayCommand DoConfirm { get; set; }
        public RelayCommand DoDelete { get; set; }
        public RelayCommand DoCancel { get; set; }

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

        public bool IsEditing
        {
            get
            {
                //if statement for user and admin
                return _isEditing;
            }
            set
            {
                _isEditing = value;
                OnPropertyChanged();
            }
        }


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
                }
                OnPropertyChanged();
            }
        }

        public User SelectedManager
        {
            get { return _selectedManager; }
            set { _selectedManager = value; OnPropertyChanged(); }
        }

        private async void Confirm()
        {
            if (CheckTextFields())
            {
                Data.AllStores[SelectedStore.ID].Name = Name;
                Data.AllStores[SelectedStore.ID].Address = Address;
                Data.AllStores[SelectedStore.ID].Phone = Phone;
                Data.AllStores[SelectedStore.ID].ManagerID = SelectedManager.Id;

                await APIHandler<Store>.PutOne("Stores/PutStore/" + SelectedStore.ID, Data.AllStores[SelectedStore.ID]);

                SelectedStore = null;

                OnPropertyChanged(nameof(StoreList));
                //StoreList.Remove(item);
                //StoreList.Add(new Store(Name, Address, Phone, Manager));

            }
        }

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

        private async void Cancel()
        {
            Name = "";
            Address = "";
            Phone = 0;
            SelectedManager = null;
            //NavigationHandler.NavigateBackwards();
        }

        private void StockPage()
        {
            RuntimeDataHandler.SelectedStore = SelectedStore;
            Frame mainFrame = Window.Current.Content as Frame;
            mainFrame?.Navigate(Type.GetType($"{Application.Current.GetType().Namespace}.Views.StoreStockPage"));
        }

        private async Task LoadDataAsync()
        {
            await Data.UpdateUsers();
            await Data.UpdateStore();

            OnPropertyChanged(nameof(StoreList));
            OnPropertyChanged(nameof(AllManagers));
        }

        public bool CheckTextFields()
        {
            bool expression = !string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(Address) &&
                               !string.IsNullOrEmpty(Phone.ToString()) && !string.IsNullOrEmpty(SelectedManager.Id.ToString());
            if (expression)
            {
                return true;
            }

            return false;
        }

        public override string ToString()
        {
            return $"{Name} {Address} {Phone}";
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
