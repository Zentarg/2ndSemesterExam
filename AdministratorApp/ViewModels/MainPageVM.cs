using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Core.Preview;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using AdministratorApp.Annotations;
using AdministratorApp.Models;
using CommonLibrary.Models;
using GalaSoft.MvvmLight.Command;

namespace AdministratorApp.ViewModels
{
    class MainPageVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public MainPageVM()
        {
            DoToggleHamburger = new RelayCommand(ToggleHamburger);
            DoLogout = new RelayCommand(Logout);

            SystemNavigationManagerPreview.GetForCurrentView().CloseRequested += this.OnCloseRequest;

        }

        public bool SplitViewVisible { get; set; }

        public string CurrentPageName => NavigationHandler.CurrentPageName;
        public bool FrameCanGoBackwards => NavigationHandler.Frame.CanGoBack;
        public bool FrameCanGoForwards => NavigationHandler.Frame.CanGoForward;
        public User ActiveUser => AuthHandler.ActiveUser;
        public bool ShowAdministratorFunctions => AuthHandler.ShowAdministratorFunctions;
        public bool DontShowAdministratorFunctions => !ShowAdministratorFunctions;

        public RelayCommand DoToggleHamburger { get; set; }
        public RelayCommand DoLogout { get; set; }

        public void ToggleHamburger()
        {
            SplitViewVisible = !SplitViewVisible;

            OnPropertyChanged(nameof(SplitViewVisible));
        }

        public void Logout()
        {
            AuthHandler.Logout();
            Frame mainFrame = Window.Current.Content as Frame;
            mainFrame?.Navigate(Type.GetType($"{Application.Current.GetType().Namespace}.LoginPage"));
        }


        private void OnCloseRequest(object sender, SystemNavigationCloseRequestedPreviewEventArgs e)
        {
            AuthHandler.Logout();
        }

        [NotifyPropertyChangedInvocator]
        public virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
