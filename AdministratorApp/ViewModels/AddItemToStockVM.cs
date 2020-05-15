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

        private Item _item;
        private Stock _stock;
        private int _amount;
        private string _errorMessage ="";

        public AddItemToStockVM()
        {
            LoadAllDataAsync();
            AddItemCommand = new RelayCommand(AddItemToStock);
            CancelCommand = new RelayCommand(NavigateBack);
        }

        public ObservableCollection<Item> Items { get => new ObservableCollection<Item>(Data.AllItems.Values);}
        public ObservableCollection<Stock> Stocks { get => new ObservableCollection<Stock>(Data.AllStocks.Values); }

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

        private async Task LoadAllDataAsync()
        {
            await Data.UpdateItems();
            await Data.UpdateStock();
            OnPropertyChanged(nameof(Items));
            OnPropertyChanged(nameof(Stocks));
        }

        public async void AddItemToStock()
        {
            if (CheckTextFields())
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
            else
            ErrorMessage = "All the fields have to be filled out.";
        }

        private bool CheckTextFields()
        {
            bool condition = SelectedItem != null && SelectedStock != null && Amount!=0;

            if ( condition == true)
            {
                return true;
            }

            return false;
        }

        private void ClearBoxes()
        {
            SelectedItem = Items[0];
            Amount = 0;
            OnPropertyChanged(nameof(SelectedItem));
            OnPropertyChanged(nameof(Amount));
        }

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
