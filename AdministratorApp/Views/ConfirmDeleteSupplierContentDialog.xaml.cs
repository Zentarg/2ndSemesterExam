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
using AdministratorApp.Models;
using AdministratorApp.ViewModels;
using CommonLibrary.Models;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace AdministratorApp.Views
{
    public sealed partial class ConfirmDeleteSupplierContentDialog : ContentDialog
    {
        private ConfirmSupplierDeleteVm vm;
        public ConfirmDeleteSupplierContentDialog()
        {
            this.InitializeComponent();
            vm = DataContext as ConfirmSupplierDeleteVm;
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }

        /// <summary>
        /// Method that deletes a supplier from the system when the confirm button is pressed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private async void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            Supplier deletedSupplier = await APIHandler<Supplier>.DeleteOne($"Suppliers/DeleteSupplier/{vm.SupplierToDelete.Id}");
            if (deletedSupplier.Id != -1)
            {
                await VMHandler.SupplierVm.LoadDataAsync();
                VMHandler.SupplierVm.FeedBackText = $"{deletedSupplier.Name} has been deleted";
                VMHandler.SupplierVm.Deselect();
            }
            else
            {
                ErrorTextBlock.Text = "Error: Unable to delete default supplier";
                args.Cancel = true;
            }
        }
    }
}
