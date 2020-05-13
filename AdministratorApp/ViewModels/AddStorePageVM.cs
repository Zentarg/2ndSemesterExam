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
        ObservableCollection<Store> _stores = new ObservableCollection<Store>();
        ObservableCollection<string> _managers = new ObservableCollection<string>();
        ObservableCollection<User> _users = new ObservableCollection<User>();
        private User _selectedManager;

        Store _store = new Store();

        private string _name = "";
        private string _address = "";
        private int _phone = 0;
        private string _errorText = "";

        public AddStorePageVM()
        {
            LoadDataAsync();
            DoCreate = new RelayCommand(Create);
            DoCancel = new RelayCommand(Cancel);
        }

        public RelayCommand DoCreate { get; set; }
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

        public string ErrorText
        {
            get { return _errorText; }
            set { _errorText = value; OnPropertyChanged(); }
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

        public ObservableCollection<Store> AllStores
        {
            get
            {
                return _stores;
            }
            set { _stores = value; OnPropertyChanged(); }
        }

        public User SelectedManager
        {
            get { return _selectedManager; }
            set
            {
                _selectedManager = value;
                OnPropertyChanged();
            }
        }

        private async void Create()
        {
            if (CheckTextFields())
            { 
                _store = new Store(Name, Address, Phone, SelectedManager.Id);
                var item = AllStores.FirstOrDefault(s =>
                    s.Name == Name && s.Address == Address && s.Phone == Phone && s.ManagerID == SelectedManager.Id);

                if (item == null)
                {
                    ErrorText = "";
                    //Store postedStore = await APIHandler<Store>.PostOne("Stores", _store);
                }
                else ErrorText = "Store already exists";

                //if (item.Name == Name && item.Address == Address && item.Phone == Phone && item.ManagerID == SelectedManager.Id)

                //var item = AllStores.FirstOrDefault(s => s.Name == Name && s.Address == Address && s.Phone == Phone && s.ManagerID == SelectedManager.Id);

            }
        }

        private void Cancel()
        {
            Name = "";
            Address = "";
            Phone = 0;
            SelectedManager = null;
            ErrorText = "";
            //NavigationHandler.NavigateBackwards();
        }

        public bool CheckTextFields()
        {
            bool expression = !string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(Address) &&
                               !string.IsNullOrEmpty(Phone.ToString()) &&
                               !string.IsNullOrEmpty(SelectedManager.Id.ToString());
            if (expression)
            {
                return true;
            }

            return false;
        }

        private async Task LoadDataAsync()
        {
            await Data.UpdateUsers();
            await Data.UpdateStore();
            AllStores = new ObservableCollection<Store>(Data.AllStores.Values);
            OnPropertyChanged(nameof(AllManagers));
            OnPropertyChanged(nameof(AllStores));
        }


        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
