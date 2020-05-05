using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using AdministratorApp.Annotations;
using CommonLibrary;

namespace AdministratorApp.ViewModels
{
    public class StockPageViewModel : INotifyPropertyChanged
    {

        public StockPageViewModel()
        {
            Stock.Add(new Item(1,"Rose",150));
            Stock.Add(new Item(2, "Lilly", 175));
            Stock.Add(new Item(3, "Tulip", 225));
        }

        public ObservableCollection<Item> Stock { get; set; } = new ObservableCollection<Item>();


        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
