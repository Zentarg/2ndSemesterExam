using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Text.Core;
using AdministratorApp.Annotations;
using AdministratorApp.Models;
using AdministratorApp.Views;
using CommonLibrary.Models;
using GalaSoft.MvvmLight.Command;

namespace AdministratorApp.ViewModels
{
    public class StockPageVM : INotifyPropertyChanged
    {
        private Tuple<Item, string> _selectedItem;
        private List<Item> _items;
        private float _priceAfterDiscount;
        private string _errorMessage;
        private string _filterString = "";
        private List<Category> _selectedCategories = new List<Category>();

        public StockPageVM()
        {
            LoadDataAsync();
            VMHandler.StockPageVm = this;
            GoToAddItem = new RelayCommand(NavigateToAddItemPage);
            DeselectItemCommand = new RelayCommand(DeselectItem);
            DeleteItemCommand=new RelayCommand(DeleteItem);
            NavigateToAddItemToStockCommand = new RelayCommand(NavigateToAddItemToStock);
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set { _errorMessage = value; OnPropertyChanged(); }
        }
        public RelayCommand DeleteItemCommand { get; }
        public RelayCommand DeselectItemCommand { get; }
        public RelayCommand GoToAddItem { get; }
        public RelayCommand NavigateToAddItemToStockCommand { get; }

        public static Dictionary<int, Dictionary<int, int>> StockHasItems
        {
            get => Data.StockHasItems;
            set => Data.StockHasItems = value;
        }

        public ObservableCollection<Category> AllCategories
        {
            get => new ObservableCollection<Category>(Data.AllCategories.Values);
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

        public string FilterString
        {
            get { return _filterString; }
            set
            {
                _filterString = value;
                OnPropertyChanged(nameof(FilteredItems));
            }
        }

        public ObservableCollection<Tuple<Item, string>> FilteredItems
        {
            get
            {
                ObservableCollection<Tuple<Item, string>> items = new ObservableCollection<Tuple<Item, string>>();
                List<Item> filteredList = new List<Item>();
                if (SelectedCategories.Count == 0)
                    filteredList = CommonMethods.FilterListByString(Data.AllItems.Values.ToList(), FilterString);
                else
                {
                    List<Tuple<Item, int>> itemsToFilter = new List<Tuple<Item, int>>();
                    foreach (Item item in Data.AllItems.Values)
                    {
                        itemsToFilter.Add(new Tuple<Item, int>(item, item.CategoryId));
                    }

                    filteredList = CommonMethods.FilterListByString(
                        CommonMethods.FilterListByCategories(itemsToFilter, SelectedCategories), FilterString);
                }

                foreach (Item item in filteredList)
                {
                    items.Add(new Tuple<Item, string>(item,
                        Data.AllCategories.Count != 0 ? Data.AllCategories[item.CategoryId].Name : "No Category."));
                }

                return items;
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
            set
            {
                Data.AllCategories[SelectedItem.Item1.CategoryId] = value ; OnPropertyChanged();
            }
        }

        public List<Category> SelectedCategories
        {
            get { return _selectedCategories; }
            set
            {
                _selectedCategories = value;
                OnPropertyChanged(nameof(FilteredItems));
            }
        }

        public ObservableCollection<KeyValuePair<Stock, int>> SelectedItemInStocks
        {
            get
            {
                ObservableCollection<KeyValuePair<Stock, int>> stocks =
                    new ObservableCollection<KeyValuePair<Stock, int>>();

                if (SelectedItem == null)
                    return stocks;
                if (Data.ItemsInStocks.ContainsKey(SelectedItem.Item1.Id))
                { foreach (KeyValuePair<int, int> pair in Data.ItemsInStocks[SelectedItem.Item1.Id])
                {
                    stocks.Add(new KeyValuePair<Stock, int>(Data.AllStocks[pair.Key], pair.Value));
                }

 
                    return stocks;
                }

                return new ObservableCollection<KeyValuePair<Stock, int>>()
                {
                    new KeyValuePair<Stock, int>(
                        new Stock(0, "This item has not been placed to any store yet.      "), 0)
                };
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

        private void DeselectItem()
        {
            SelectedItem = null;
        }

        public async Task<bool> SaveEdit()
        {
            try
            {
                if (CheckFields())
                {
                    if (SelectedItem.Item1.DiscountPercentage <= 100)
                    {
                        await APIHandler<Item>.PutOne($"items/{SelectedItem.Item1.Id}",
                            new Item(SelectedItem.Item1.Id, SelectedItem.Item1.Name, SelectedItem.Item1.Price,
                                SelectedItem.Item1.Comment, SelectedItem.Item1.PictureSource,
                                SelectedItem.Item1.Barcode,
                                SelectedItem.Item1.Color, SelectedItem.Item1.Size, SelectedItemCategory.ID,
                                SelectedItem.Item1.DiscountPercentage));
                        await LoadDataAsync();
                        return true;
                    }
                    else ErrorMessage = "Discount cannot be more than 100%";

                    return false;
                }

               else ErrorMessage = "All required fields must be filled out!"; return false;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private bool CheckFields()
        {
            bool Experssion = !string.IsNullOrEmpty(SelectedItem.Item1.Name) && SelectedItem.Item1.Price != 0 &&
                              SelectedItem.Item1.Barcode != 0 && !string.IsNullOrEmpty(SelectedItem.Item1.Color) &&
                              !string.IsNullOrEmpty(SelectedItem.Item1.Size);

            if (Experssion)
            {
                return true;
            }

            return false;
        }

        public async void DeleteItem()
        {
            if (SelectedItem != null)
            {
                await APIHandler<Item>.DeleteOne($"items/{SelectedItem.Item1.Id}");
                await Data.UpdateItems();
                OnPropertyChanged(nameof(FilteredItems));
                SelectedItem = FilteredItems[0];
            }

        }




        private async Task LoadDataAsync()
        {
            await Data.UpdateCategories();
            await Data.UpdateItems();
            await Data.UpdateStock();
            await Data.UpdateStore();
            await Data.UpdateItemsInStocks();
            OnPropertyChanged(nameof(SelectedItemCategory));
            OnPropertyChanged(nameof(AllCategories));
            OnPropertyChanged(nameof(FilteredItems));
            OnPropertyChanged(nameof(Stores));
            OnPropertyChanged(nameof(Stocks));
            OnPropertyChanged(nameof(SelectedItemInStocks));
            OnPropertyChanged(nameof(PriceAfterDiscount));
            SelectedItem = FilteredItems[0];
            
        }

        private void NavigateToAddItemToStock()
        {
            NavigationHandler.NavigateToPage(typeof(AddItemToStockPage));
        }

        private void NavigateToAddItemPage()
        {
            NavigationHandler.NavigateToPage(typeof(AddItemPage));
        }

        

        


        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
