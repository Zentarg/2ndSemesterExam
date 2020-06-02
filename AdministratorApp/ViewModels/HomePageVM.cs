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
    class HomePageVM : INotifyPropertyChanged
    {

        public HomePageVM()
        {
            LoadDataAsync();
        }

        public bool ShowAdministratorFunctions => AuthHandler.ShowAdministratorFunctions;
        public bool DontShowAdministratorFunctions => !ShowAdministratorFunctions;

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public int NoItems
        {
            get => Data.AllItems.Count;
        }
        public int NoStores
        {
            get => Data.AllStores.Count;
        }

        public int NoEmployees
        {
            get => Data.AllUsers.Count;
        }

        public int NoSuppliers
        {
            get => Data.AllSuppliers.Count;
        }

        public int NoRequests
        {
            get
            {
                int No = 0;
                foreach (var request in Data.AllInvoices.Values)
                {
                    if (request.InvoiceStatusID == 0)
                    {
                        No++;
                    }
                }

                return No;
            }
        }

        public async Task LoadDataAsync()
        {
            await Data.UpdateItems();
            await Data.UpdateStore();
            await Data.UpdateUsers();
            await Data.UpdateSuppliers();
            await Data.UpdateInvoices();
            OnPropertyChanged(nameof(NoItems));
            OnPropertyChanged(nameof(NoEmployees));
            OnPropertyChanged(nameof(NoStores));
            OnPropertyChanged(nameof(NoSuppliers));
            OnPropertyChanged(nameof(NoRequests));
        }
    }
}
