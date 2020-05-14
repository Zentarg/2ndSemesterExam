using System;
using System.Collections.Generic;
using System.Drawing;
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
using AdministratorApp.Models;
using AdministratorApp.ViewModels;
using CommonLibrary.Models;
using Size = System.Drawing.Size;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace AdministratorApp.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class StockPage : Page
    {
        private StockPageViewModel _vm;
        public StockPage()
        {
            this.InitializeComponent();
            _vm = DataContext as StockPageViewModel;
        }

        private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (StockListView.SelectedItem != null)
            {
                ItemPanel.Visibility = Visibility.Visible;
            }
        }

        private void EditButton_OnClick(object sender, RoutedEventArgs e)
        {
            SimpleItemDisplay.Visibility = Visibility.Collapsed;
            EditableItemDisplay.Visibility = Visibility.Visible;
        }


        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            SimpleItemDisplay.Visibility = Visibility.Visible;
            EditableItemDisplay.Visibility = Visibility.Collapsed;
        }

        private async void SaveButton_OnClick(object sender, RoutedEventArgs e)
        {
            Item newItem = new Item(Convert.ToInt32(IdText.Text), Convert.ToString(NameInput.Text),
                Convert.ToSingle(PriceInput.Text), Convert.ToString(CommentInput.Text), PictureInput.Text,
                Convert.ToInt32(BarcodeInput.Text), Convert.ToString(ColorInput.Text), Convert.ToString(SizeInput.Text),
                Convert.ToInt32(CategoryInput.Text),
                Convert.ToSingle(DiscountInput.Text));
           await APIHandler<Item>.PostOne("items", newItem );
        }

    }
}
