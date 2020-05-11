using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdministratorApp.Models
{
    public class SelectedStoreSingleton
    {
        private SelectedStoreSingleton _instance;
        private Store _selectedStore;

        private SelectedStoreSingleton() 
        { }

        public SelectedStoreSingleton Instance
        {
            get
            {
                if (_instance==null) _instance = new SelectedStoreSingleton();
                return _instance;
            }
        }

        public Store SelectedStore { get =>_selectedStore;
            set => _selectedStore = value;
        }
    }
}
