using System;
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

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace AdministratorApp.Views
{
    public sealed partial class ConfirmSupplierEditContentDialog : ContentDialog
    {
        private ConfirmSupplierEditVM vm;
        public ConfirmSupplierEditContentDialog()
        {
            this.InitializeComponent();
            vm = DataContext as ConfirmSupplierEditVM;
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }

        /// <summary>
        /// Method that closes the dialog box and updates the supplier
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private async void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            await vm.PutSupplier();
            
            VMHandler.SupplierVm.CancelEdit();
            VMHandler.SupplierVm.FeedBackText = "Supplier Updated Successfully";
            await VMHandler.SupplierVm.LoadDataAsync();
        }
    }
}
