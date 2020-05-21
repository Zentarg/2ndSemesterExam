using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using AdministratorApp.Annotations;
using AdministratorApp.Models;
using CommonLibrary.Models;

namespace AdministratorApp.ViewModels
{
   public class AddSupplierVM : INotifyPropertyChanged
    {
        private Supplier _newSupplier = new Supplier();
        private string _errorMessage;

        public AddSupplierVM()
        {
            VMHandler.AddSupplierVm = this;
        }

        public Supplier NewSupplier
        {
            get {

                return _newSupplier;
            }
            set
            {
                _newSupplier = value;
                OnPropertyChanged();
            }
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set{ _errorMessage = value;
            OnPropertyChanged();
        }
    }

        public ObservableCollection<Supplier> AllSuppliers { get => new ObservableCollection<Supplier>(Data.AllSuppliers.Values);}

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// This method checks the inserted information, to make sure all fields are filled out.
        /// </summary>
        /// <returns></returns>
        private bool CheckFields()
        {
            bool expression = !string.IsNullOrEmpty(NewSupplier.Name) && !string.IsNullOrEmpty(NewSupplier.Address) &&
                              !string.IsNullOrEmpty(NewSupplier.Email) && NewSupplier.Phone != 0;
            if (expression)
            {
                return true;
            }
            else ErrorMessage = "All fields have to be filled out."; return false;
        }

        private bool CheckIfAlreadyExist(string name)
        {
            foreach (var Supplier in AllSuppliers)
            {
                if (Supplier.Name == name)
                {
                    ErrorMessage = $"Supplier with the following name: {NewSupplier.Name}, already exists.";
                    return false;
                }
            }
            return true;
        }

        public async Task<bool> AddSupplier()
        {
            if (CheckFields())
            {
                if (CheckIfAlreadyExist(_newSupplier.Name))
                {
                    var addedSupplier = await APIHandler<Supplier>.PostOne("suppliers",
                    new Supplier(_newSupplier.Name, _newSupplier.Address, _newSupplier.Email, _newSupplier.Phone));
                    return true;
                }

                return false;
            }

            return false;

        }

        private async Task LoadDataAsync()
        {
            await  Data.UpdateSuppliers();
            OnPropertyChanged(nameof(AllSuppliers));
        }

    }
}
