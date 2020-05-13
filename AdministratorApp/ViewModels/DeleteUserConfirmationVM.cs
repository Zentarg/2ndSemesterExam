using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using AdministratorApp.Annotations;
using AdministratorApp.Models;
using CommonLibrary.Models;
using GalaSoft.MvvmLight.Command;

namespace AdministratorApp.ViewModels
{
    public class DeleteUserConfirmationVM : INotifyPropertyChanged
    {
        private User _user;
        private string _errorText = "";

        public DeleteUserConfirmationVM()
        {
            SelectedEmp = Data.SelectedUser;

        }


        public User SelectedEmp
        {
            get { return _user; }
            set { _user = value; OnPropertyChanged(); }
        }

        public string ErrorText
        {
            get { return _errorText; }
            set { _errorText = value; OnPropertyChanged(); }
        }

        public Constants.UserDeleteErorrs ErrorCheck()
        {
            int id = SelectedEmp.Id;
            if (SelectedEmp != null)
            {
                if (SelectedEmp.Id != AuthHandler.UserID)
                {
                    if (SelectedEmp.UserLevelId != Data.OwnerID)
                    {
                        if (AuthHandler.ActiveUser.UserLevelId > SelectedEmp.UserLevelId)
                        {
                            if (SelectedEmp.Id != 0)
                            {
                                return Constants.UserDeleteErorrs.OK;
                            }
                            return Constants.UserDeleteErorrs.DELETE_ID_0;
                        }
                        return Constants.UserDeleteErorrs.LOW_ACCESS_LEVEL;
                    }
                    return Constants.UserDeleteErorrs.DELETE_OWNER;
                }
                return Constants.UserDeleteErorrs.USER_LOGGED_IN;
            }
            return Constants.UserDeleteErorrs.NO_SELECTED_USER;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
