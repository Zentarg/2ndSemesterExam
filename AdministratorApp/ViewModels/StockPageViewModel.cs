using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Text.Core;
using AdministratorApp.Annotations;
using AdministratorApp.Models;
using CommonLibrary.Models;

namespace AdministratorApp.ViewModels
{
    public class StockPageViewModel : INotifyPropertyChanged
    {
        private Item _selectedItem;
        private List<Item> _items;


        public StockPageViewModel()
        {
            LoadDataAsync();
        }

        public static Dictionary<int, Dictionary<int, int>> StockHasItems { get => Data.StockHasItems; set => Data.StockHasItems = value; }

        public Item SelectedItem
        {
            get => _selectedItem;
            set { _selectedItem = value; OnPropertyChanged(); OnPropertyChanged(nameof(SelectedItemInStocks)); }
        }



        public ObservableCollection<Item> Items
        {
            get => new ObservableCollection<Item>(Data.AllItems.Values);
        }

        public ObservableCollection<Store> Stores
        {
            get => new ObservableCollection<Store>(Data.AllStores.Values);
        }

        public ObservableCollection<Stock> Stocks
        {
            get => new ObservableCollection<Stock>(Data.AllStocks.Values);
        }

        public ObservableCollection<KeyValuePair<Stock, int>> SelectedItemInStocks
        {
            get
            {
                ObservableCollection<KeyValuePair<Stock, int>> stocks = new ObservableCollection<KeyValuePair<Stock, int>>();

                if (SelectedItem == null)
                    return stocks;
                foreach (KeyValuePair<int, int> pair in Data.ItemsInStocks[SelectedItem.Id])
                {
                    stocks.Add(new KeyValuePair<Stock, int>(Data.AllStocks[pair.Key],pair.Value));
                }

                return stocks;
            }
        }

        /*public ObservableCollection<KeyValuePair<Item, Dictionary<Store, int>>> ItemsInStocks
        {
            get
            {
                ObservableCollection<KeyValuePair<Item, Dictionary<Store, int>>> itemsInStocks = new ObservableCollection<KeyValuePair<Item, Dictionary<Store, int>>>();
                foreach (KeyValuePair<int, Dictionary<int, int>> item in Data.ItemsInStocks)
                {
                    itemsInStocks.Add(new KeyValuePair<Item, Dictionary<Store, int>>());
                    foreach (KeyValuePair<int, int> store in item.Value)
                    {
                        itemsInStocks[item.Key].Value.Add(Data.AllStores[store.Key], store.Value);
                    }
                }

                return itemsInStocks;
            }
        }*/


        /*public Dictionary<int, Item> Items { 
            get => Data.AllItems.Values.;
            set { Data.AllItems = value; OnPropertyChanged();}
        }
        public Dictionary<int, Store> Stores
        {
            get => Data.AllStores;
            set { Data.AllStores = value; OnPropertyChanged(); }
        }
        public Dictionary<int, Stock> Stocks
        {
            get => Data.AllStocks;
            set { Data.AllStocks = value; OnPropertyChanged(); }
        }*/


        private async Task LoadDataAsync()
        {
            await Data.UpdateItems();
            await Data.UpdateStock();
            await Data.UpdateStore();
            await Data.UpdateItemsInStocks();
            OnPropertyChanged(nameof(Items));
            OnPropertyChanged(nameof(Stores));
            OnPropertyChanged(nameof(Stocks));
            OnPropertyChanged(nameof(SelectedItemInStocks));
            SelectedItem = Items[0];
        }

        


        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
