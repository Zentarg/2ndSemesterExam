using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Authentication.OnlineId;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using AdministratorApp.Annotations;
using AdministratorApp.Models;
using GalaSoft.MvvmLight.Command;

namespace AdministratorApp.ViewModels
{
    class LoginPageVM : INotifyPropertyChanged
    {

        public LoginPageVM()
        {
            DoAttemptLogin = new RelayCommand(AttemptLogin);
        }

        public string Username { get; set; }
        public string Password { get; set; }

        public bool DisplayLoginWindow { get; set; } = true;
        public bool DisplayLoading { get; set; } = false;
        public bool DisplayErrorWindow { get; set; } = false;
        public string ErrorWindowText { get; set; } = "";

        public RelayCommand DoAttemptLogin { get; set; }

        /// <summary>
        /// Attempts to login using information from properties.
        /// </summary>
        public async void AttemptLogin()
        {
            DisplayLoginWindow = false;
            DisplayLoading = true;
            OnPropertyChanged(nameof(DisplayLoginWindow));
            OnPropertyChanged(nameof(DisplayLoading));
            string passwordSalt = await APIHandler<string>.GetOne($"Auth/GetSalt/{Username}");

            if (passwordSalt != null)
            {
                (int UserID, string SessionKey) sessionTuple =
                    await APIHandler<(int UserID, string SessionKey)>.GetOne($"Auth/Login/{Username}/{AuthHandler.EncryptPassword(Password, passwordSalt)}");

                if (sessionTuple != default)
                {
                    AuthHandler.UserID = sessionTuple.UserID;
                    AuthHandler.SessionKey = sessionTuple.SessionKey;
                    await AuthHandler.InitializeAuth();
                    Frame mainFrame = Window.Current.Content as Frame;
                    mainFrame?.Navigate(Type.GetType($"{Application.Current.GetType().Namespace}.MainPage"));
                }
                else
                {
                    DisplayLoginError(Constants.LOGIN_ERROR);
                }
            }
            else
            {
                DisplayLoginError(Constants.LOGIN_ERROR);
            }

        }

        /// <summary>
        /// Displays error box with string errortext.
        /// </summary>
        /// <param name="errorText">Error text to display.</param>
        public void DisplayLoginError(string errorText)
        {
            DisplayLoading = false;
            ErrorWindowText = errorText;
            DisplayErrorWindow = true;
            DisplayLoginWindow = true;
            Password = "";

            OnPropertyChanged(nameof(Password));
            OnPropertyChanged(nameof(DisplayLoading));
            OnPropertyChanged(nameof(ErrorWindowText));
            OnPropertyChanged(nameof(DisplayErrorWindow));
            OnPropertyChanged(nameof(DisplayLoginWindow));
        }


        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
