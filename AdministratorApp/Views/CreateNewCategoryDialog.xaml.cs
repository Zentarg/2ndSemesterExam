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
    public sealed partial class CreateNewCategoryDialog : ContentDialog
    {
        private CreateNewCategoryVM viewModel;

        public CreateNewCategoryDialog()
        {
            this.InitializeComponent();
            viewModel = DataContext as CreateNewCategoryVM;

        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }

        private async void ContentDialog_OnConfirmClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            try
            {
                if (viewModel.CheckErrors())
                {
                    Category c =  await APIHandler<Category>.PostOne("categories", new Category(EnterCategoryBox.Text));
                    VMHandler.AddItemViewModel.LoadDataAsync();
                    VMHandler.AddItemViewModel.Category = c;
                    
                    args.Cancel = false;
                }
                else args.Cancel = true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }
    }
}
