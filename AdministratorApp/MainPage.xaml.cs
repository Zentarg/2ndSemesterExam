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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace AdministratorApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private MainPageVM _mainPageVM;

        public MainPage()
        {
            this.InitializeComponent();
            NavigationHandler.Frame = MainFrame;
            _mainPageVM = DataContext as MainPageVM;
        }

        private void NavigateFrame(object sender, RoutedEventArgs e)
        {
            NavigationHandler.NavigateToPage(Type.GetType($"{Application.Current.GetType().Namespace}.Views.{(sender as Button).Tag}"));
            _mainPageVM.OnPropertyChanged(nameof(_mainPageVM.CurrentPageName));

        }

        private void NavigateBackwards(object sender, RoutedEventArgs e)
        {
            NavigationHandler.NavigateBackwards();
            _mainPageVM.OnPropertyChanged(nameof(_mainPageVM.FrameCanGoBackwards));
        }

        private void NavigateForwards(object sender, RoutedEventArgs e)
        {
            NavigationHandler.NavigateForwards();
            _mainPageVM.OnPropertyChanged(nameof(_mainPageVM.FrameCanGoForwards));
        }
    }
}
