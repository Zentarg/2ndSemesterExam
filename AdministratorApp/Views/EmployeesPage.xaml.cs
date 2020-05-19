using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.UserDataTasks;
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
    public sealed partial class EmployeesPage : Page
    {
        public EmployeesPage()
        {
            this.InitializeComponent();
        }

        private void NavigateFrame(object sender, RoutedEventArgs e)
        {
            NavigationHandler.NavigateToPage(Type.GetType($"{Application.Current.GetType().Namespace}.Views.{(sender as Button).Tag}"));
        }

        /// <summary>
        /// Method that is called on every key press for a text box that has this method assigned to it on the BeforeTextChangeEvent
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void OnlyNumberTextBox(TextBox sender, TextBoxBeforeTextChangingEventArgs args)
        {
            args.Cancel = args.NewText.Any(c => !char.IsDigit(c));
        }

        /// <summary>
        /// Method that is called on every key press for a text box that has this method assigned to it on the BeforeTextChangeEvent
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void OnlyFloatTextBox(TextBox sender, TextBoxBeforeTextChangingEventArgs args)
        {
            if (!string.IsNullOrEmpty(args.NewText))
                if (!float.TryParse(args.NewText, out float f))
                    args.Cancel = true;
        }

        /// <summary>
        /// Method that is called when a user clicks on create new role, that opens a content dialog to create a new role
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void OpenCreateNewRoleContentDialog(object sender, RoutedEventArgs e)
        {
            CreateNewRoleContentDialog cNRCD = new CreateNewRoleContentDialog();
            await cNRCD.ShowAsync();
        }
    }
}
