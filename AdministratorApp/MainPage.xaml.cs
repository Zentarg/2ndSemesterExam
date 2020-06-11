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
using Windows.Globalization;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace AdministratorApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        public MainPage()
        {
            this.InitializeComponent();
            NavigationHandler.Frame = MainFrame;
            NavigationHandler.NavigateToPage(Type.GetType($"{Application.Current.GetType().Namespace}.Views.HomePage"));
        }

        private void NavigateFrame(object sender, RoutedEventArgs e)
        {
            NavigationHandler.NavigateToPage(Type.GetType($"{Application.Current.GetType().Namespace}.Views.{(sender as Button).Tag}"));
        }

        private void NavigateBackwards(object sender, RoutedEventArgs e)
        {
            NavigationHandler.NavigateBackwards();
        }

        private void NavigateForwards(object sender, RoutedEventArgs e)
        {
            NavigationHandler.NavigateForwards();
        }

        private void LanguageButtonHU_OnClick(object sender, RoutedEventArgs e)
        {
            ApplicationLanguages.PrimaryLanguageOverride = "hu";
            Frame.Navigate(this.GetType());
        }

        private void LanguageButtonEN_OnClick(object sender, RoutedEventArgs e)
        {
            ApplicationLanguages.PrimaryLanguageOverride = "en-US";
            Frame.Navigate(this.GetType());
        }
    }
}
