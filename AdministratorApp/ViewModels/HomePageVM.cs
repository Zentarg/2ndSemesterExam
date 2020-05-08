using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using AdministratorApp.Annotations;
using AdministratorApp.Models;

namespace AdministratorApp.ViewModels
{
    class HomePageVM : INotifyPropertyChanged
    {

        public HomePageVM()
        {
            
        }

        public bool ShowAdministratorFunctions => AuthHandler.ShowAdministratorFunctions;
        public bool DontShowAdministratorFunctions => !ShowAdministratorFunctions;

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
