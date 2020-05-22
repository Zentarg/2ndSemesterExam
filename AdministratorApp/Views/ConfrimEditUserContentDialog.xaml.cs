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
    public sealed partial class ConfrimEditUserContentDialog : ContentDialog
    {
        private ConfirmEditUserVM vm;
        public ConfrimEditUserContentDialog()
        {
            this.InitializeComponent();
            vm = DataContext as ConfirmEditUserVM;

        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }

        /// <summary>
        /// Method that is called when a user clicks on confirm in the dialogue box and confirms the edit of the user
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private async void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            Constants.PutErrors error = vm.CanUserUpdate();
            if (error == Constants.PutErrors.OK)
            {
                await vm.PutUser();
                VMHandler.EmployeesPageVm.FeedBackText = "User updated successfully";
                VMHandler.EmployeesPageVm.CloseEdit();
                VMHandler.EmployeesPageVm.Deselect();
                VMHandler.EmployeesPageVm.LoadDataAsync();
                args.Cancel = false;
            }
            else
            {
                FeedBackTextBlock.Text = "User couldn't be updated: Email already in use";
                args.Cancel = true;
            }
        }
    }
}
