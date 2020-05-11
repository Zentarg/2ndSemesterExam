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
    public sealed partial class CreateNewRoleContentDialog : ContentDialog
    {
        private CreateNewRoleVM vm;
        public CreateNewRoleContentDialog()
        {
            this.InitializeComponent();
            vm = DataContext as CreateNewRoleVM;
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }

        private async void ContentDialog_OnConfirmClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            var error = vm.CheckRoleForErrors();
            ErrorTextBlock.Text = vm.SetErrorText(error);
            if (error == Constants.RoleErrors.OK)
            {
                await APIHandler<Role>.PostOne("Roles", new Role(0, EnterRoleBox.Text));
                await Data.UpdateRoles();
                args.Cancel = false;
            }
            args.Cancel = true;
        }
    }
}
