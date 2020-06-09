using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using AdministratorApp.Annotations;
using AdministratorApp.Models;
using CommonLibrary.Models;

namespace AdministratorApp.ViewModels
{
    class LogsPageVM : INotifyPropertyChanged
    {
        private ObservableCollection<Log> _logEntries = new ObservableCollection<Log>();
        public LogsPageVM()
        {
            LoadDataAsync();

        }

        public ObservableCollection<Log> LogEntries
        {
            get { return _logEntries; }
            set { _logEntries = value; OnPropertyChanged(); }
        }

        public string LogEntry { get; set; }

        public async void LoadDataAsync()
        {
            await Data.UpdateLogs();
            LogEntries = new ObservableCollection<Log>(Data.AllLogs);
        }


        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
