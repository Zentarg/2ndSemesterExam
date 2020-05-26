using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    public sealed partial class StockPage : Page
    {
        private StockPageVM _vm;
        private StockPageVM _viewModel;
        public StockPage()
        {
            this.InitializeComponent();
            _viewModel = VMHandler.StockPageVm;
            _vm = DataContext as StockPageVM;
            HideItemPanel();
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

        private void CategoriesGrid_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            List<Category> selectedCategories = new List<Category>();
            foreach (object item in (sender as GridView).SelectedItems)
            {
                selectedCategories.Add(item as Category);
                Debug.WriteLine(item);
            }

            _vm.SelectedCategories = selectedCategories;
        }

        private void StocksGrid_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            List<Stock> selectedStocks = new List<Stock>();
            foreach (object item in (sender as GridView).SelectedItems)
            {
                selectedStocks.Add(item as Stock);
                Debug.WriteLine(item);
            }

            _vm.SelectedStocks = selectedStocks;
        }

        private void FilterNonNumeric_OnBeforeTextChanging(TextBox sender, TextBoxBeforeTextChangingEventArgs args)
        {
            args.Cancel = args.NewText.Any(c => !char.IsDigit(c));
        }

        private async void SaveButton_OnClick(object sender, RoutedEventArgs e)
        {
            bool SaveWasSuccessful = await _viewModel.SaveEdit();
            if (SaveWasSuccessful)
            {
                EditableItemDisplay.Visibility = Visibility.Collapsed;
                SimpleItemDisplay.Visibility = Visibility.Visible;
            }
        }

        private async void DeleteButton_OnClick(object sender, RoutedEventArgs e)
        {

            ContentDialog dialog = new ContentDialog()
            {
                Title = "Delete item",
                Content =
                    $"Are you sure that you would like to delete {_viewModel.SelectedItem.Item1.Name} permanently from the system?\nThis will delete the item from each and every stock.",
                PrimaryButtonText = "Delete",
                CloseButtonText = "Close",
                PrimaryButtonCommand = _viewModel.DeleteItemCommand
            };
            await dialog.ShowAsync();
        }

        private void HideItemPanel()
        {
            ItemPanel.Visibility = Visibility.Collapsed;
        }

        private void CloseButton_OnClick(object sender, RoutedEventArgs e)
        {
            _viewModel.DeselectItemCommand.Execute(_viewModel.DeselectItemCommand);
            ItemPanel.Visibility = Visibility.Collapsed;
        }
    }
}
