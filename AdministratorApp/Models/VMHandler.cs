using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdministratorApp.ViewModels
{
    public static class VMHandler
    {
        /// <summary>
        /// Stores the CreateEmployee View model so that it can be referenced when creating a new role
        /// </summary>
        public static CreateEmployeeVM CreateEmployeeVm { get; set; }

        public static AddItemVM AddItemViewModel { get; set; }

        /// <summary>
        /// Stores the Employees Page View model so that it can be referenced when confirming the delete or edit of a user, and when creating a new role
        /// </summary>
        public static EmployeesPageVM EmployeesPageVm { get; set; }

        public static StockPageVM StockPageVm { get; set; }

        public static AddSupplierVM AddSupplierVm { get; set; }
        public static SupplierVM SupplierVm { get; set; }
    }
}
