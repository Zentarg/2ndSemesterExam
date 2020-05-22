using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using AdministratorApp.Annotations;
using AdministratorApp.Models;
using CommonLibrary.Models;
using GalaSoft.MvvmLight.Command;

namespace AdministratorApp.ViewModels
{
    class CreateInvoicePageVM : INotifyPropertyChanged
    {
        private Store _selectedStore = null;
        private ObservableCollection<Tuple<Item, InvoiceHasItem>> _itemsToChange = new ObservableCollection<Tuple<Item, InvoiceHasItem>>();
        private string _comment = "";
        private string _price = "";
        private string _discount = "";

        public CreateInvoicePageVM()
        {
            LoadData();
            DoCancel = new RelayCommand(Cancel);
            DoConfirm = new RelayCommand(Confirm);
        }

        public ObservableCollection<Store> AllStores
        {
            get => new ObservableCollection<Store>(Data.AllStores.Values);
        }

        public Store SelectedStore
        {
            get => _selectedStore;
            set
            {
                _selectedStore = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(StockHasItems));
                OnPropertyChanged(nameof(CanEdit));
                ItemsToChange.Clear();
            }
        }

        public string Comment
        {
            get => _comment;
            set
            {
                _comment = value;
                OnPropertyChanged();
            }
        }
        public string Price
        {
            get => _price;
            set
            {
                _price = value;
                OnPropertyChanged();
            }
        }
        public string Discount
        {
            get => _discount;
            set
            {
                _discount = value;
                OnPropertyChanged();
            }

        }

        public RelayCommand DoCancel { get; set; }
        public RelayCommand DoConfirm { get; set; }


        public void Cancel()
        {
            SelectedStore = null;
            Comment = "";
            Price = "";
            Discount = "";
            OnPropertyChanged(nameof(StockHasItems));
            OnPropertyChanged(nameof(CanEdit));
        }

        public async void Confirm()
        {
            Invoice newInvoice = new Invoice(0, AuthHandler.UserID, float.Parse(Price), float.Parse(Discount), Comment, SelectedStore.ID, (int) Constants.InvoiceStatus.Open, "");
            List<InvoiceHasItem> invoiceHasItems = new List<InvoiceHasItem>();
            Invoice invoice = await APIHandler<Invoice>.PostOne("Invoices", newInvoice);
            foreach (Tuple<Item, InvoiceHasItem> tuple in ItemsToChange)
            {
                await APIHandler<InvoiceHasItem>.PostOne("InvoiceHasItems", new InvoiceHasItem(invoice.ID, tuple.Item1.Id, tuple.Item2.Amount));
            }


            SelectedStore = null;
            Comment = "";
            Price = "";
            Discount = "";
            OnPropertyChanged(nameof(StockHasItems));
            OnPropertyChanged(nameof(CanEdit));


        }

        public bool CanEdit
        {
            get => SelectedStore != null;
        }

        public ObservableCollection<Item> StockHasItems
        {
            get
            {
                ObservableCollection<Item> items = new ObservableCollection<Item>();

                if (SelectedStore == null)
                    return items;

                foreach (KeyValuePair<int, int> pair in Data.StockHasItems[SelectedStore.StockId])
                {
                    items.Add(Data.AllItems[pair.Key]);
                }

                return items;
            }
        }

        public ObservableCollection<Tuple<Item, InvoiceHasItem>> ItemsToChange
        {
            get => _itemsToChange;
            set
            {
                _itemsToChange = value;
                OnPropertyChanged();
            }
        }

        public async Task LoadData()
        {
            await Data.UpdateItems();
            await Data.UpdateStore();
            await Data.UpdateStockHasItems();
            OnPropertyChanged(nameof(AllStores));
            OnPropertyChanged(nameof(StockHasItems));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
