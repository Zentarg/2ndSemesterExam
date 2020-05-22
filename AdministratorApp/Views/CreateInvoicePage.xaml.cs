using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using AdministratorApp.ViewModels;
using CommonLibrary.Models;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace AdministratorApp.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CreateInvoicePage : Page
    {
        private CreateInvoicePageVM _vm;
        public CreateInvoicePage()
        {
            this.InitializeComponent();
            _vm = DataContext as CreateInvoicePageVM;
        }

        private void FilterNonNumeric_OnBeforeTextChanging(TextBox sender, TextBoxBeforeTextChangingEventArgs args)
        {
            args.Cancel = args.NewText.Any(c => !char.IsDigit(c));
        }

        private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ObservableCollection<Tuple<Item, InvoiceHasItem>> selectedItems = new ObservableCollection<Tuple<Item, InvoiceHasItem>>();
            foreach (object item in (sender as ListView).SelectedItems)
            {
                selectedItems.Add(new Tuple<Item, InvoiceHasItem>(item as Item, new InvoiceHasItem(0, (item as Item).Id,0)));
            }

            _vm.ItemsToChange = selectedItems;

        }
    }
}
