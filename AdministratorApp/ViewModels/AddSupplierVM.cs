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
        //Instance fields
        private Supplier _newSupplier = new Supplier();
        private string _errorMessage;

        //Constructor of AddSupplierViewModel for sending the ViewModel to VMHandler.
        public AddSupplierVM()
        {
            VMHandler.AddSupplierVm = this;
            LoadDataAsync();
        }

        //Reference type property for the new supplier.
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

        //Value type string property for error messages.
        public string ErrorMessage
        {
            get => _errorMessage;
            set{ _errorMessage = value;
            OnPropertyChanged();
        }
    }

        //
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

        /// <summary>
        /// This method checks if supplier with the given name already exists.
        /// </summary>
        /// <param name="name">The name of the supplier we would like to check.</param>
        /// <returns>Returns a true or false value depending on if the supplier already exist. Returns false if the supplier already exist.</returns>
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


        /// <summary>
        /// This method is responsible for adding new supplier to the database asynchronously with the help of the APIHandler.
        /// </summary>
        /// <returns>Returns a true or false value depending on if the supplier was added successfully</returns>
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

        /// <summary>
        /// Method responsible for loading necessary data from Database, with the help of the Data class.
        /// </summary>
        /// <returns></returns>

        private async Task LoadDataAsync()
        {
            await  Data.UpdateSuppliers();
            OnPropertyChanged(nameof(AllSuppliers));
        }

    }
}
