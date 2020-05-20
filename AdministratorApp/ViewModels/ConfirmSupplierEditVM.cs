using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdministratorApp.Models;
using CommonLibrary.Models;

namespace AdministratorApp.ViewModels
{
    public class ConfirmSupplierEditVM
    {
        /// <summary>
        /// Constructor that loads all the properties up with the correct information
        /// </summary>
        public ConfirmSupplierEditVM()
        {
            LoadDataAsync();
            BeforeEdit = Data.SelectedSupplier;
            AfterEdit = Data.EditedSupplier;
        }

        public Supplier BeforeEdit { get; set; }
        public Supplier AfterEdit { get; set; }

        /// <summary>
        /// A method that updates all the data in the Data.cs file that is relevant to editing a supplier
        /// </summary>
        public async void LoadDataAsync()
        {
            await Data.UpdateSuppliers();
        }

        /// <summary>
        /// Method that updates a supplier in the system via the API
        /// </summary>
        /// <returns></returns>
        public async Task PutSupplier()
        {
            await APIHandler<Supplier>.PutOne($"Suppliers/UpdateSupplier/{BeforeEdit.Id}", AfterEdit);
        }
    }
}
