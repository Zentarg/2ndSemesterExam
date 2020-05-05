using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using AdministratorApp.Annotations;
using AdministratorApp.Models;
using GalaSoft.MvvmLight.Command;

namespace AdministratorApp.ViewModels
{
    class MainPageVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public MainPageVM()
        {
            DoToggleHamburger = new RelayCommand(ToggleHamburger);
        }

        public bool SplitViewVisible { get; set; }

        public string CurrentPageName => NavigationHandler.CurrentPageName;
        public bool FrameCanGoBackwards => NavigationHandler.Frame.CanGoBack;
        public bool FrameCanGoForwards => NavigationHandler.Frame.CanGoBack;


        public RelayCommand DoToggleHamburger { get; set; }

        public void ToggleHamburger()
        {
            SplitViewVisible = !SplitViewVisible;

            OnPropertyChanged(nameof(SplitViewVisible));
        }

        [NotifyPropertyChangedInvocator]
        public virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
