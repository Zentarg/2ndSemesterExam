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

namespace AdministratorApp.ViewModels
{
   public class StoreStockPageVM : INotifyPropertyChanged
    {

        public StoreStockPageVM()
        {
            LoadDataAsync();

        }

        //public Stock SelectedStock { get => RuntimeDataHandler.SelectedStock; }
        public Stock SelectedStock { get; set; }


        public ObservableCollection<KeyValuePair<Item,int>> Items {
            get
            {
                ObservableCollection<KeyValuePair<Item,int>> items = new ObservableCollection<KeyValuePair<Item, int>>();
                foreach (var product in Data.StockHasItems[SelectedStock.StockID])
                {
                    items.Add(new KeyValuePair<Item, int>(Data.AllItems[product.Key], product.Value ));
                }

                return items;
            }
        }

        public async Task LoadDataAsync()
        {
            await Data.UpdateItems();
            await Data.UpdateStockHasItems();
            await Data.UpdateStock();
            OnPropertyChanged(nameof(SelectedStock));
        }


        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
