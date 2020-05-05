using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using AdministratorApp.Annotations;
using AdministratorApp.Models;

namespace AdministratorApp.ViewModels
{
    public class StorePageVM : INotifyPropertyChanged
    {
        private Store _store;
        private Store _selectedStore;

        public StorePageVM()
        {
            

        }


        public List<Store> StoreList
        {
            get { return _store.StoreList;}
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

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
