using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        ObservableCollection<Store> _stores = new ObservableCollection<Store>();
        private string _name = "";
        private string _address = "";
        private int _phone = 0;
        private string _manager = "";

        public AddStorePageVM()
        {
            DoCreate = new RelayCommand(Create);
            DoCancel = new RelayCommand(Cancel);
        }

        public RelayCommand DoCreate { get; set; }
        public RelayCommand DoCancel { get; set; }

        public string Name
        {
            get { return _name;}
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


        private void Create()
        {
            if (Name != null && Address != null && Phone>=00000001 && Manager != null)
            {
                // Add to database
               _stores.Add(new Store(Name, Address, Phone, Manager));
            }
        }

        private void Cancel()
        {
            Name = "";
            Address = "";
            Phone = 0;
            Manager = "";
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
