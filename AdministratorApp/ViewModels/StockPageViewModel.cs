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


        public Item SelectedItem
        {
            get => _selectedItem;
            set { _selectedItem = value; OnPropertyChanged(); }
        }
        public List<Item> Stock
        {
            get => _items;
            private set { _items = value; OnPropertyChanged(); }
        }

        public List<Item> Items { 
            get => Data.AllItems;
            set { Data.AllItems = value; OnPropertyChanged();}
        }

        public Dictionary<int,int> ItemsInStock { get; set; }

        private async Task LoadDataAsync()
        {
            await Data.UpdateItems();
            OnPropertyChanged(nameof(Items));
        }

        

        private async Task GetProductAmount(int id)
        {
            //Amount
        }


        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
