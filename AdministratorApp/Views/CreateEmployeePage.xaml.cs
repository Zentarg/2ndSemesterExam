using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
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
    public sealed partial class CreateEmployeePage : Page
    {
        public CreateEmployeePage()
        {
            this.InitializeComponent();
        }

        private void NavigateFrame(object sender, RoutedEventArgs e)
        {
            NavigationHandler.NavigateToPage(Type.GetType($"{Application.Current.GetType().Namespace}.Views.{(sender as Button).Tag}"));
        }

        private async void OpenCreateNewRoleContentDialog(object sender, RoutedEventArgs e)
        {
            CreateNewRoleContentDialog cNRCD = new CreateNewRoleContentDialog();
            await cNRCD.ShowAsync();
        }


        private void OnlyNumberTextBox(TextBox sender, TextBoxBeforeTextChangingEventArgs args)
        {
            args.Cancel = args.NewText.Any(c => !char.IsDigit(c));
        }

        private void OnlyFloatTextBox(TextBox sender, TextBoxBeforeTextChangingEventArgs args)
        {
            string allowedChars = "-1234567890.";
            bool checkForDots = (args.NewText.IndexOf('.') != args.NewText.LastIndexOf('.'));
            args.Cancel = args.NewText.Any(c => !allowedChars.Contains(c)) || checkForDots;
        }
    }
}
