using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdministratorApp.Models;
using CommonLibrary.Models;

namespace AdministratorApp.ViewModels
{
    public class ConfirmSupplierDeleteVm
    {
        public ConfirmSupplierDeleteVm()
        {
            SupplierToDelete = Data.SelectedSupplier;
        }

        public Supplier SupplierToDelete
        {
            get;
            set;
        }
    }
}
