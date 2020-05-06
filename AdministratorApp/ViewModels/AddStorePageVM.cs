using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using AdministratorApp.Annotations;
using AdministratorApp.Models;
using GalaSoft.MvvmLight.Command;

namespace AdministratorApp.ViewModels
{
    public class AddStorePageVM : INotifyPropertyChanged
    {
        private Store _store;
        private RelayCommand _addStore;

        public AddStorePageVM()
        {
            
        }

        public string Name
        {
            get { return _store.Name;}
            set { _store.Name = value; OnPropertyChanged(); }
        }

        public string Address
        {
            get { return _store.Address; }
            set { _store.Address = value; OnPropertyChanged(); }
        }

        public int Telephone
        {
            get { return _store.Telephone; }
            set { _store.Telephone = value; OnPropertyChanged(); }
        }

        public string Manager
        {
            get { return _store.Manager; }
            set { _store.Manager = value; OnPropertyChanged(); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
