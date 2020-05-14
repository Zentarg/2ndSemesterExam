using CommonLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdministratorApp.Models;

namespace AdministratorApp.ViewModels
{
    public class ConfirmEditUserVM
    {
        private User _beforeEdit;
        private User _afterEdit;

        public ConfirmEditUserVM()
        {
            BeforeEdit = Data.SelectedUser;
            AfterEdit = Data.EditedUser;
            RoleBefore = CommonMethods.GetRole(BeforeEdit.RoleId, Data.AllRoles);
            RoleAfter = CommonMethods.GetRole(AfterEdit.RoleId, Data.AllRoles);
            AccountLevelBefore = CommonMethods.GetUserLevel(BeforeEdit.UserLevelId, Data.AllLevels);
            AccountLevelAfter = CommonMethods.GetUserLevel(AfterEdit.UserLevelId, Data.AllLevels);
            StoreBefore = CommonMethods.GetStore(BeforeEdit.StoreId, Data.AllStores);
            StoreAfter = CommonMethods.GetStore(AfterEdit.StoreId, Data.AllStores);
            SalaryBefore = CommonMethods.GetSalary(BeforeEdit.Id, Data.AllSalaries);
            SalaryAfter = Data.EditedSalary;
            SalaryWTaxBefore = SalaryBefore.BeforeTax - (SalaryBefore.BeforeTax * (SalaryBefore.TaxPercentage / 100));
            SalaryWTaxAfter = SalaryAfter.BeforeTax - (SalaryAfter.BeforeTax * (SalaryAfter.TaxPercentage / 100));
        }

        public User BeforeEdit { get; set; }
        public User AfterEdit { get; set; }
        public Role RoleBefore { get; set; }
        public Role RoleAfter { get; set; }
        public UserLevel AccountLevelBefore { get; set; }
        public UserLevel AccountLevelAfter { get; set; }
        public Store StoreBefore { get; set; }
        public Store StoreAfter { get; set; }
        public Salary SalaryBefore { get; set; }
        public Salary SalaryAfter { get; set; }
        public float SalaryWTaxBefore { get; set; }
        public float SalaryWTaxAfter { get; set; }

        public async void LoadDataAsync()
        {
            await Data.UpdateRoles();
            await Data.UpdateUserLevels();
            await Data.UpdateStore();
            await Data.UpdateSalaries();
        }
    }
}
