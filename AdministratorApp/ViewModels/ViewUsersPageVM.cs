using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AdministratorApp.ViewModels
{
    class ViewUsersPageVM : INotifyPropertyChanged
    {
        List<string> _emp = new List<string>() { "one", "two", "three" };
        string _sEmp;
        public ViewUsersPageVM()
        {

        }

        public List<string> Emp
        {
            get { return _emp; }
        }

        public string SelectedEmp
        {
            set { _sEmp = value;  }
        }



        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
