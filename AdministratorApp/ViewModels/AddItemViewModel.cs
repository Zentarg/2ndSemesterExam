using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdministratorApp.Models;
using CommonLibrary.Models;
using GalaSoft.MvvmLight.Command;

namespace AdministratorApp.ViewModels
{
    class AddItemViewModel 
    {
        public AddItemViewModel()
        {
            AddItemCommand = new RelayCommand(AddItem);
        }

        public ObservableCollection<Store> Stores
        {
            get => new ObservableCollection<Store>(Data.AllStores.Values);
        }

        public string Name { get; set; }
        public float Price { get; set; }
        public int Discount { get; set; }
        public string StoreName { get; set; }
        public int StoreID { get; set; }
        public int Category { get; set; }
        public int CategoryID { get; set; }
        public string Color { get; set; }
        public string Size { get; set; }
        public string Comment { set; get; }
        public string PictureUrl { get; set; }
        public int Barcode { get; set; }

        public RelayCommand AddItemCommand;

        public void AddItem()
        {
            Store store = Stores.FirstOrDefault(s => s.Name == StoreName);
            StoreID = store.ID;
            Item newItem = new Item(0,Name,Price,Comment,PictureUrl,Barcode,Color,Size,CategoryID,Discount);

        }



    }
}
