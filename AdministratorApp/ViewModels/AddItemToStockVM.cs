using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.WindowManagement;
using Windows.UI.Xaml.Controls;
using AdministratorApp.Annotations;
using AdministratorApp.Models;
using CommonLibrary.Models;
using GalaSoft.MvvmLight.Command;

namespace AdministratorApp.ViewModels
{
    class AddItemToStockVM : INotifyPropertyChanged
    {
        //Instance fields
        private Item _item;
        private Stock _stock;
        private int _amount;
        private string _errorMessage ="";

        //Constructor to LoadAllData and to initialise RelayCommands
        public AddItemToStockVM()
        {
            LoadAllDataAsync();
            AddItemCommand = new RelayCommand(AddItemToStock);
            CancelCommand = new RelayCommand(NavigateBack);
        }

        public ObservableCollection<Item> Items { get => new ObservableCollection<Item>(Data.AllItems.Values);}
        public ObservableCollection<Stock> Stocks { get => new ObservableCollection<Stock>(Data.AllStocks.Values); }
        public static Dictionary<int, Dictionary<int, int>> StockHasItems
        {
            get => Data.StockHasItems;
        }

        public Item SelectedItem
        {
            get => _item;set { _item = value; OnPropertyChanged(); }
        }
        public Stock SelectedStock
        {
            get => _stock; set { _stock = value; OnPropertyChanged(); }
        }
        public int Amount { get => _amount; set { _amount = value; OnPropertyChanged(); }
        }

        public string ErrorMessage
        {
            get { return _errorMessage; }
            set
            {
                _errorMessage = value;
                OnPropertyChanged();
            }
        } 

        public RelayCommand CancelCommand { get; set; }

        public RelayCommand AddItemCommand { get; set; }


        /// <summary>
        /// Method for loading required Data from the Database and update properties.
        /// </summary>
        private async Task LoadAllDataAsync()
        {
            await Data.UpdateItems();
            await Data.UpdateStock();
            await Data.UpdateStockHasItems();
            OnPropertyChanged(nameof(Items));
            OnPropertyChanged(nameof(Stocks));
            OnPropertyChanged(nameof(StockHasItems));
        }

        /// <summary>
        /// This method is responsible for adding a new item to a stock. This method uses the PostOne method of the APIHandler.
        /// </summary>
        public async void AddItemToStock()
        {
            
            if (CheckTextFields())
            {
                if (CheckIfItemAlreadyAdded())
                {
                    
              
                if (Amount>0)
                {
                    ErrorMessage = "";
                await APIHandler<StockHasItems>.PostOne("stockhasitems", new StockHasItems(SelectedStock.ID, SelectedItem.Id, Amount));
                ContentDialog dialog = new ContentDialog()
                {
                    Title = "Item successfully added!",
                    Content = $"{Amount}pcs. of {SelectedItem.Name} was successfully added to {SelectedStock.Name}!",
                    PrimaryButtonText = "Ok"
                };
                await dialog.ShowAsync();
                ClearBoxes();
                }
                else
                {
                    ErrorMessage = "The amount cannot be 0 or negative.";
                }

                }
            }
            else
            ErrorMessage = "All the fields have to be filled out.";
        }

        /// <summary>
        /// This method is responsible for checking if an item has been already added to a specific store.
        /// </summary>
        /// <returns>Returns a true or false value depending on if the item already exist in the store. Returns false if the item has already been added.</returns>
        private bool CheckIfItemAlreadyAdded()
        {
            foreach (var stockId in StockHasItems.Keys)
            {
                if (stockId == SelectedStock.ID)
                {
                    foreach (var Item in StockHasItems[stockId])
                    {
                        if (Item.Key == SelectedItem.Id)
                        {
                            ErrorMessage = "The item has already been added to this store.";
                            return false;

                        }   
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// This method checks if all the required fields are filled out.
        /// </summary>
        /// <returns> Return a true or false value depending on if all the required fields are filled out.
        /// Returns true if all required field is filled out, false if not.</returns>
        private bool CheckTextFields()
        {
            bool condition = SelectedItem != null && SelectedStock != null && Amount!=0;

            if ( condition == true)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// This method resets the textboxes on the page.
        /// </summary>
        private void ClearBoxes()
        {
            SelectedStock = null;
            SelectedItem = null;
            Amount = 0;
            OnPropertyChanged(nameof(SelectedItem));
            OnPropertyChanged(nameof(Amount));
        }

        /// <summary>
        /// This method is responsible for navigating back to the previous page. It is using the Navigation handler class.
        /// </summary>
        private void NavigateBack()
        {
            NavigationHandler.NavigateBackwards();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
