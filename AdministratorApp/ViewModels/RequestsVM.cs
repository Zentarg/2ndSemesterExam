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
    class RequestsVM : INotifyPropertyChanged
    {

        public RequestsVM()
        {
            
        }


        public List<Store> SelectedStores { get; set; }

        public ObservableCollection<Store> AllStores
        {
            get
            {
                return new ObservableCollection<Store>(Data.AllStores.Values);
            }
        }

        public ObservableCollection<Invoice> FilteredInvoices
        {
            get { return new ObservableCollection<Invoice>(Data.AllInvoices.Values); }
        }


        public async Task LoadData()
        {
            await Data.UpdateInvoices();
            await Data.UpdateStoreHasInvoices();
            await Data.UpdateInvoiceHasItems();
            await Data.UpdateStore();
            OnPropertyChanged(nameof(FilteredInvoices));
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
