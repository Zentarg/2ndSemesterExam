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
    public sealed partial class DeleteUserConfirmationContentDialog : ContentDialog
    {
        private DeleteUserConfirmationVM vm;
        public DeleteUserConfirmationContentDialog()
        {
            this.InitializeComponent();
            vm = DataContext as DeleteUserConfirmationVM;
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }

        private async void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            Constants.UserDeleteErorrs error = vm.ErrorCheck();
            ErrorTextBlock.Text = CommonMethods.SetErrorTextOnDelete(error);
            if (error == Constants.UserDeleteErorrs.OK)
            {
                User user = await APIHandler<User>.DeleteOne($"Users/DeleteUser/{vm.SelectedEmp.Id}");
                await VMHandler.EmployeesPageVm.LoadDataAsync();
                VMHandler.EmployeesPageVm.FeedBackText = $"{user.Name} has been deleted";
                args.Cancel = false;
            }
            else
                args.Cancel = true;
        }
    }
}
