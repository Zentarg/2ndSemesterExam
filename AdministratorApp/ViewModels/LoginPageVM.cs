using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Command;

namespace AdministratorApp.ViewModels
{
    class LoginPageVM
    {

        public LoginPageVM()
        {
            
        }

        public string Username { get; set; }
        public string Password { get; set; }

        public RelayCommand DoAttemptLogin { get; set; }

        public void AttemptLogin()
        {

        }


    }
}
