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

        public ConfirmEditUserVM()
        {
            LoadDataAsync();
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

        public Constants.EmailCheckErrors IsEmailInUse()
        {
            if (AfterEdit.Email == BeforeEdit.Email)
                if (Data.AllUsers.Values.Any(u => u.Email == BeforeEdit.Email))
                    return Constants.EmailCheckErrors.EMAIL_NOT_EDITED;

            if (AfterEdit.Email != BeforeEdit.Email)
                if (Data.AllUsers.Values.Any(u => u.Email == AfterEdit.Email))
                    return Constants.EmailCheckErrors.EMAIL_IN_USE;

            return Constants.EmailCheckErrors.OK;
        }

        public Constants.PutErrors CanUserUpdate()
        {
            if (IsEmailInUse() == Constants.EmailCheckErrors.EMAIL_NOT_EDITED || IsEmailInUse() == Constants.EmailCheckErrors.OK)
            {
                return Constants.PutErrors.OK;
            }
            return Constants.PutErrors.CONTENT_DID_NOT_PUT;
        }

        public async Task PutUser()
        {
            await APIHandler<Salary>.PutOne($"Salaries/UpdateSalary/{BeforeEdit.Id}", SalaryAfter);
            await APIHandler<User>.PutOne($"Users/UpdateUser/{BeforeEdit.Id}", AfterEdit);
        }
    }
}
