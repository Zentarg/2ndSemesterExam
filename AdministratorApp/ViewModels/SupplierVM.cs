using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using AdministratorApp.Annotations;
using AdministratorApp.Models;
using CommonLibrary.Models;

namespace AdministratorApp.ViewModels
{
    class SupplierVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public SupplierVM()
        {
            LoadDataAsync();
        }

        public ObservableCollection<Supplier> Suppliers
        {
            get
            {
                if (Data.AllSuppliers == null)
                {
                    return  new ObservableCollection<Supplier>();
                }
               return new ObservableCollection<Supplier>(Data.AllSuppliers.Values);
            }
        }


        private async Task LoadDataAsync()
        {
            await  Data.UpdateSuppliers();
            OnPropertyChanged(nameof(Suppliers));
        }


        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
