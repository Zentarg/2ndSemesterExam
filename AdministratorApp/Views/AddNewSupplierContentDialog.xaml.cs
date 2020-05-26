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


        /// <summary>
        /// Method which is called when the primary button of the content dialog is pressed. This method is responsible for
        /// calling the AddSupplier method of the ViewModel.
        /// </summary>
        private async void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            bool successfulAddSupplier =await viewModel.AddSupplier();

            if (successfulAddSupplier)
            {
                ContentDialog successContentDialog = new ContentDialog();
                successContentDialog.Title = $"{NameInput.Text} successfully added!";
                successContentDialog.PrimaryButtonText = "Ok";
                await successContentDialog.ShowAsync();
                await supplierViewModel.LoadDataAsync();
            }
            else
            {
                args.Cancel = true;
            }
        }

        /// <summary>
        /// Method which is called when the secondary button of the content dialog is pressed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }

        /// <summary>
        /// This method is filtering the input of the TextBox to prevent non-numeric input.
        /// </summary>
        private void TextBox_OnBeforeTextChanging(TextBox sender, TextBoxBeforeTextChangingEventArgs args)
        {
            args.Cancel = args.NewText.Any(c => !char.IsDigit(c));
        }
    }
}
