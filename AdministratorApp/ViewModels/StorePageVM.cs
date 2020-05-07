using AdministratorApp.Annotations;
using AdministratorApp.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using CommonLibrary.Models;

namespace AdministratorApp.ViewModels
{
    public class StorePageVM : INotifyPropertyChanged
    {
        private Store _store;
        private Store _selectedStore;
        private bool _isEditing;
        private string _name;
        private string _address;
        private int _phone;
        private string _manager;

        public StorePageVM()
        {
            LoadDataAsync();

        }

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

        public int Telephone
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
                ObservableCollection<Store> stores = new ObservableCollection<Store>();
                //Dictionary<int, Store> dictionary = new Dictionary<int, Store>(Data.AllStores);
                foreach (Store store in Data.AllStores.Values)
                {
                    stores.Add(store);
                }
                return stores;

            }
            //get
            //{
            //    return new List<Store>()
            //{
            //    new Store(0,0,0,"John Doe's", "Some place", 84758439, "John Doe"),
            //    new Store(1,1,1,"Your Mom", "Your Mom's", 123456789, "Your Dad's")
            //};
            //}
        }

        public Store SelectedStore
        {
            get { return _selectedStore; }
            set
            {
                _selectedStore = value;
                
                OnPropertyChanged();
            }
        }

        private async Task LoadDataAsync()
        {
            await Data.UpdateStore();
            OnPropertyChanged(nameof(StoreList));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
