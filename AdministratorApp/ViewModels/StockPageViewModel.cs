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
        private Tuple<Item, string> _selectedItem;
        private List<Item> _items;
        private float _priceAfterDiscount;
        

        public StockPageViewModel()
        {
            LoadDataAsync();
        }

        public static Dictionary<int, Dictionary<int, int>> StockHasItems { get => Data.StockHasItems; set => Data.StockHasItems = value; }

        public Tuple<Item, string> SelectedItem
        {
            get => _selectedItem;
            set { _selectedItem = value; OnPropertyChanged(); OnPropertyChanged(nameof(SelectedItemInStocks)); OnPropertyChanged(nameof(PriceAfterDiscount)); OnPropertyChanged(nameof(SelectedItemCategory)); }
        }


        public float PriceAfterDiscount
        {
            get
            {
                if (SelectedItem !=null)
                { //You can only pay integer amounts in hungary therefore it's rounded to int.
                    return _priceAfterDiscount = Convert.ToInt32(SelectedItem.Item1.Price * (1 - SelectedItem.Item1.DiscountPercentage / 100)) ;
                }

                return 0;
            }
        }
        //TUPLE format
        public ObservableCollection<Tuple<Item, string>> Items
        {
            get
            {
                ObservableCollection<Tuple<Item, string>> items = new ObservableCollection<Tuple<Item, string>>();
                foreach (Item item in Data.AllItems.Values)
                {
                    items.Add(new Tuple<Item, string>(item, Data.AllCategories[item.CategoryId].Name));
                }

                return items;
            }
        }

        public ObservableCollection<Store> Stores
        {
            get => new ObservableCollection<Store>(Data.AllStores.Values);
        }

        public ObservableCollection<Stock> Stocks
        {
            get => new ObservableCollection<Stock>(Data.AllStocks.Values);
        }

        public Category SelectedItemCategory
        {
            get => SelectedItem != null ? Data.AllCategories[SelectedItem.Item1.CategoryId] : new Category(-1, "loading");
        }

        public ObservableCollection<KeyValuePair<Stock, int>> SelectedItemInStocks
        {
            get
            {
                ObservableCollection<KeyValuePair<Stock, int>> stocks = new ObservableCollection<KeyValuePair<Stock, int>>();

                if (SelectedItem == null)
                    return stocks;
                foreach (KeyValuePair<int, int> pair in Data.ItemsInStocks[SelectedItem.Item1.Id])
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
            await Data.UpdateCategories();
            OnPropertyChanged(nameof(Items));
            OnPropertyChanged(nameof(Stores));
            OnPropertyChanged(nameof(Stocks));
            OnPropertyChanged(nameof(SelectedItemInStocks));
            OnPropertyChanged(nameof(PriceAfterDiscount));
            OnPropertyChanged(nameof(SelectedItemCategory));
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
