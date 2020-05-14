using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace AdministratorApp.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class StorePage : Page
    {
        public StorePage()
        {
            this.InitializeComponent();
        }

        private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (StoreListView.SelectedItem != null)
            {
                DeleteButton.IsEnabled = true;
                ConfirmButton.IsEnabled = true;
            }
            else
            {
                DeleteButton.IsEnabled = false;
                ConfirmButton.IsEnabled = false;
            }

        }

        private void DeselectButton_OnClick(object sender, RoutedEventArgs e)
        {
            StoreListView.SelectedItem = null;
        }

        private void EditButton_OnClick(object sender, RoutedEventArgs e)
        {
            SimpleStackPanel.Visibility = Visibility.Collapsed;
            EditableStackPanel.Visibility = Visibility.Visible;
            EditButton.Visibility = Visibility.Collapsed;
            EditableButtons.Visibility = Visibility.Visible;
        }

        private void CancelButton_OnClick(object sender, RoutedEventArgs e)
        {
            SimpleStackPanel.Visibility = Visibility.Visible;
            EditableStackPanel.Visibility = Visibility.Collapsed;
            EditButton.Visibility = Visibility.Visible;
            EditableButtons.Visibility = Visibility.Collapsed;
        }
    }
}
