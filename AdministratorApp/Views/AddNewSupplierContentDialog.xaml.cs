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
    public sealed partial class AddNewSupplierContentDialog : ContentDialog
    {
        private AddSupplierVM viewModel;
        private SupplierVM supplierViewModel;
        public AddNewSupplierContentDialog()
        {
            this.InitializeComponent();
            viewModel = VMHandler.AddSupplierVm;
            supplierViewModel = VMHandler.SupplierVm;
        }

        private async void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            bool successfulAddSupplier =await viewModel.AddSupplier();
            if (successfulAddSupplier == false)
            {
                args.Cancel = true;
            }
            else
            {
                ContentDialog successContentDialog = new ContentDialog();
                successContentDialog.Title = $"{NameInput.Text} successfully added!";
                successContentDialog.PrimaryButtonText = "Ok";
                await successContentDialog.ShowAsync();

                await supplierViewModel.LoadDataAsync();
            }

        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
           
        }

        private void TextBox_OnBeforeTextChanging(TextBox sender, TextBoxBeforeTextChangingEventArgs args)
        {
            args.Cancel = args.NewText.Any(c => !char.IsDigit(c));
        }
    }
}
