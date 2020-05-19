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
using AdministratorApp.Models;

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

        /// <summary>
        /// Disables confirm and delete buttons when nothing is selected in the list of stores
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Deselects the list of stores
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeselectButton_OnClick(object sender, RoutedEventArgs e)
        {
            StoreListView.SelectedItem = null;
        }

        /// <summary>
        /// Collapses the edit button and makes confirm, delete, cancel and add store page buttons visible
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditButton_OnClick(object sender, RoutedEventArgs e)
        {
            SimpleStackPanel.Visibility = Visibility.Collapsed;
            EditableStackPanel.Visibility = Visibility.Visible;
            EditButton.Visibility = Visibility.Collapsed;
            EditableButtons.Visibility = Visibility.Visible;
            AddStore.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Collapses confirm, delete, cancel, add store page buttons and makes edit button visible
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CancelButton_OnClick(object sender, RoutedEventArgs e)
        {
            SimpleStackPanel.Visibility = Visibility.Visible;
            EditableStackPanel.Visibility = Visibility.Collapsed;
            EditButton.Visibility = Visibility.Visible;
            EditableButtons.Visibility = Visibility.Collapsed;
            AddStore.Visibility = Visibility.Collapsed;
        }
    }
}
