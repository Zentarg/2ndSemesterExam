using System;
using AdministratorApp.Annotations;
using AdministratorApp.Models;
using GalaSoft.MvvmLight.Command;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using AdministratorApp.Views;
using CommonLibrary.Models;

namespace AdministratorApp.ViewModels
{
    public class StorePageVM : INotifyPropertyChanged
    {
        private Store _store = new Store();
        private Store _selectedStore = new Store();
        private bool _isEditing = false;
        private string _name = "";
        private string _address = "";
        private int _phone = 0;
        private string _manager = "";

        //ObservableCollection<Store> stores = new ObservableCollection<Store>(){ new Store("John Doe's", "Some place", 84758439, "John Doe"), new Store("Your Mom","Your Mom's", 123456789, "Your Dad's")};

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
                //Dictionary<int, Store> dictionary = new Dictionary<int, Store>(Data.AllStores);

                ObservableCollection<Store> stores = new ObservableCollection<Store>(Data.AllStores.Values);
                

                if (stores.Count > 0)
                {
                    if (stores[0].ID == 0)
                    {
                        stores.RemoveAt(0);
                    }
                }
                return stores;

                //}
                //get
                //{
                //    return stores;
                //}
            }
        }

        public ObservableCollection<User> AllUsers
        {
            get
            {
                ObservableCollection<User> users = new ObservableCollection<User>(Data.AllUsers.Values);
                return users;
            }
        }

        public ObservableCollection<string> AllUserNames
        {
            get
            {
                ObservableCollection<string> userNames = new ObservableCollection<string>();
                foreach (User user in AllUsers)
                {
                    userNames.Add(user.Name);
                }

                return userNames;
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
                    Manager = _selectedStore.Manager;
                }
                OnPropertyChanged();
            }
        }

        private async void Confirm()
        {
            if (Name != null && Address != null && Phone >= 00000001 && Manager != null)
            {

                _selectedStore.Name = Name;
                _selectedStore.Address = Address;
                _selectedStore.Phone = Phone;
                _selectedStore.Manager = Manager;

                var item = StoreList.SingleOrDefault(s => s.Name == Name && s.Address == Address && s.Phone == Phone && s.Manager == Manager);
                APIHandler<Store>.PostOne("Stores", item);

                //StoreList.Remove(item);
                //StoreList.Add(new Store(Name, Address, Phone, Manager));


                OnPropertyChanged(nameof(StoreList));
            }
        }

        private async void Delete()
        {
            if (SelectedStore != null)
            {
                StoreList.Remove(SelectedStore);
                OnPropertyChanged(nameof(StoreList));
                Cancel();
            }
        }

        private async void Cancel()
        {
            Name = "";
            Address = "";
            Phone = 0;
            Manager = "";
            NavigationHandler.NavigateBackwards();
        }

        private async void StockPage()
        {
            Frame mainFrame = Window.Current.Content as Frame;
            mainFrame?.Navigate(Type.GetType($"{Application.Current.GetType().Namespace}.Views.StoreStockPage"));
            //RuntimeDataHandler.SelectedStore = SelectedStore;
        }

        private async Task LoadDataAsync()
        {
            await Data.UpdateStore();
            await Data.UpdateUsers();
            OnPropertyChanged(nameof(StoreList));
        }

        public override string ToString()
        {
            return $"{Name} {Address} {Phone} {Manager}";
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
