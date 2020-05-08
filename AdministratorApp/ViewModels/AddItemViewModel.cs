using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonLibrary.Models;
using GalaSoft.MvvmLight.Command;

namespace AdministratorApp.ViewModels
{
    class AddItemViewModel : INotifyPropertyChanged
    {
        public AddItemViewModel()
        {
            AddItemCommand = new RelayCommand(AddItem);
        }
        public string Name { get; set; }
        public float Price { get; set; }
        public int Dicount { get; set; }
        public string Store { get; set; }
        public int StoreID { get; set; }
        public int Category { get; set; }
        public int CategoryID { get; set; }
        public string Color { get; set; }
        public string Size { get; set; }
        public string Comment { set; get; }

        public RelayCommand AddItemCommand;

        public void AddItem()
        {

        }


        event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
        {
            add
            {
                throw new NotImplementedException();
            }

            remove
            {
                throw new NotImplementedException();
            }
        }
    }
}
