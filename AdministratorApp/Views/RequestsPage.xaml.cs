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
using AdministratorApp.Models;
using AdministratorApp.ViewModels;
using CommonLibrary.Models;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace AdministratorApp.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class RequestsPage : Page
    {
        private RequestsVM _vm;
        public RequestsPage()
        {
            this.InitializeComponent();
            _vm = DataContext as RequestsVM;
        }

        private void StoresGrid_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            List<Store> selectedStores = new List<Store>();
            foreach (object item in (sender as GridView).SelectedItems)
            {
                selectedStores.Add(item as Store);
                Debug.WriteLine(item);
            }

            _vm.SelectedStores = selectedStores;
        }
    }
}
