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
    }
}
