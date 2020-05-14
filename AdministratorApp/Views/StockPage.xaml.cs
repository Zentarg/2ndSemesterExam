﻿using System;
using System.Collections.Generic;
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
    }
}
