using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Store;
using AdministratorApp.Annotations;
using AdministratorApp.Models;
using CommonLibrary.Models;
using GalaSoft.MvvmLight.Command;

namespace AdministratorApp.ViewModels
{
   public class StoreStockPageVM : INotifyPropertyChanged
    {

        private Tuple<Item, string> _selectedItem;
        private List<Item> _items;
        private float _priceAfterDiscount;
        private string _filterString = "";
        private Stock _selectedStock;


        public StoreStockPageVM()
        {
            LoadDataAsync();

        }

        public RelayCommand DeselectItemCommand { get; }

        public RelayCommand GoToAddItem { get; }
        public RelayCommand NavigateToAddItemToStockCommand { get; }

        public static Dictionary<int, Dictionary<int, int>> ItemsInStocks
        {
            get => Data.ItemsInStocks;
            set => Data.ItemsInStocks = value;
        }

        public Tuple<Item, string> SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(SelectedItemInStocks));
                OnPropertyChanged(nameof(PriceAfterDiscount));
                OnPropertyChanged(nameof(SelectedItemCategory));
            }
        }

        public float PriceAfterDiscount
        {
            get
            {
                if (SelectedItem != null)
                {
                    //You can only pay integer amounts in hungary therefore it's rounded to int.
                    return _priceAfterDiscount =
                        Convert.ToInt32(SelectedItem.Item1.Price * (1 - SelectedItem.Item1.DiscountPercentage / 100));
                }

                return 0;
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
            get => SelectedItem != null
                ? Data.AllCategories[SelectedItem.Item1.CategoryId]
                : new Category(-1, "loading");
        }

        public ObservableCollection<KeyValuePair<Stock, int>> SelectedItemInStocks
        {
            get
            {
                ObservableCollection<KeyValuePair<Stock, int>> stocks =
                    new ObservableCollection<KeyValuePair<Stock, int>>();

                if (SelectedItem == null)
                    return stocks;
                foreach (KeyValuePair<int, int> pair in Data.ItemsInStocks[SelectedItem.Item1.Id])
                {
                    stocks.Add(new KeyValuePair<Stock, int>(Data.AllStocks[pair.Key], pair.Value));
                }

                return stocks;
            }
        }

                public string FilterString
        {
            get { return _filterString; }
            set { _filterString = value; OnPropertyChanged(nameof(FilteredItems)); }
        }

        public ObservableCollection<Tuple<Item, string>> FilteredItems
        {
            get
            {
                ObservableCollection<Tuple<Item, string>> items = new ObservableCollection<Tuple<Item, string>>();
                List<Item> filteredList = CommonMethods.FilterListByString(Data.AllItems.Values.ToList(), FilterString);
                foreach (Item item in filteredList)
                {
                    items.Add(new Tuple<Item, string>(item, Data.AllCategories.Count != 0 ? Data.AllCategories[item.CategoryId].Name : "No Category." ));
                }
                return items;
            }
        }

        public ObservableCollection<KeyValuePair<Item,int>> Items {
            get
            {
                ObservableCollection<KeyValuePair<Item,int>> items = new ObservableCollection<KeyValuePair<Item, int>>();
                if (SelectedStock != null)
                {
                    foreach (var product in Data.ItemsInStocks[SelectedStock.StockID])
                    {
                        items.Add(new KeyValuePair<Item, int>(Data.AllItems[product.Key], product.Value));
                    }
                }


                return items;
            }
        }

        public Stock SelectedStock
        {
            get => _selectedStock;
            set { _selectedStock = value; OnPropertyChanged(); OnPropertyChanged(nameof(Items)); }
        }


        public async Task LoadDataAsync()
        {
            await Data.UpdateItems();
            await Data.UpdateItemsInStocks();
            await Data.UpdateStock();
            OnPropertyChanged(nameof(Items)); OnPropertyChanged(nameof(Stocks));
            OnPropertyChanged(nameof(ItemsInStocks));
            OnPropertyChanged(nameof(RuntimeDataHandler.SelectedStock));
        }




        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
