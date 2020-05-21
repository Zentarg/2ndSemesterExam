using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using AdministratorApp.Annotations;
using AdministratorApp.Models;
using AdministratorApp.Views;
using CommonLibrary.Models;
using GalaSoft.MvvmLight.Command;

namespace AdministratorApp.ViewModels
{
    public class SupplierVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private Supplier _selectedSupplier;

        private string _name;
        private int _phone;
        private string _email;
        private string _address;

        private bool _showEdit = false;
        private bool _showNormal = true;
        private bool _supplierIsSelected = false;
        private bool _showErrorField = false;
        private string _feedBackText;

        public SupplierVM()
        {
            LoadDataAsync();
            VMHandler.SupplierVm = this;
            DoCancel = new RelayCommand(CancelEdit);
            DoConfirmEdit = new RelayCommand(ConfirmEditMethod);
            DoShowEdit = new RelayCommand(ShowEditMethod);
            DoDelete = new RelayCommand(ConfirmDeleteMethod);
        }

        public RelayCommand DoCancel { get; set; }
        public RelayCommand DoDelete { get; set; }
        public RelayCommand DoShowEdit { get; set; }
        public RelayCommand DoConfirmEdit { get; set; }

        public ObservableCollection<Supplier> Suppliers
        {
            get
            {
                if (Data.AllSuppliers == null)
                {
                    return  new ObservableCollection<Supplier>();
                }
               return new ObservableCollection<Supplier>(Data.AllSuppliers.Values);
            }
        }

        public Supplier SelectedSupplier { get => _selectedSupplier;
            set { _selectedSupplier = value;
                if (_selectedSupplier != null)
                {
                    Name = _selectedSupplier.Name;
                    Address = _selectedSupplier.Address;
                    Email = _selectedSupplier.Email;
                    Phone = _selectedSupplier.Phone;
                    FeedBackText = "";
                    SupplierIsSelected = true;
                    ShowErrorField = false;
                }
                OnPropertyChanged();}
        }

        public bool ShowEdit
        {
            get { return _showEdit; }
            set { _showEdit = value;  OnPropertyChanged();}
        }

        public bool ShowErrorField
        {
            get { return _showErrorField; }
            set { _showErrorField = value; OnPropertyChanged(); }
        }

        public bool ShowNormal
        {
            get { return _showNormal; }
            set { _showNormal = value; OnPropertyChanged(); }
        }

        public string FeedBackText
        {
            get { return _feedBackText; }
            set { _feedBackText = value; OnPropertyChanged(); }
        }

        public bool SupplierIsSelected 
        { get
        {
            return _supplierIsSelected;
        }
            set { _supplierIsSelected = value; OnPropertyChanged(); }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; OnPropertyChanged(); }
        }

        public string Address
        {
            get { return _address; }
            set { _address = value; OnPropertyChanged();}
        }

        public string Email
        {
            get { return _email; }
            set { _email = value; OnPropertyChanged(); }
        }

        public int Phone
        {
            get { return _phone; }
            set { _phone = value; OnPropertyChanged(); }
        }

        public async Task LoadDataAsync()
        {
            await  Data.UpdateSuppliers();
            OnPropertyChanged(nameof(Suppliers));
        }

        /// <summary>
        /// Method that closes editing
        /// </summary>
        public void CloseEdit()
        {
            ShowNormal = true;
            ShowEdit = false;
        }

        /// <summary>
        /// method that cancels editing
        /// </summary>
        public void CancelEdit()
        {
            CloseEdit();
            FeedBackText = "";
            ShowErrorField = false;
        }

        /// <summary>
        /// method that confirms the inputted information and opens a confirmation window
        /// </summary>
        public async void ConfirmEditMethod()
        {
            FeedBackText = "";
            var errorType = CheckEditFields();
            SetFeedBackText(errorType);
            if (errorType == Constants.SupplierCheckErrors.OK)
            {
                Data.SelectedSupplier = SelectedSupplier;
                Data.EditedSupplier = new Supplier(SelectedSupplier.Id, Name, Address, Email, Phone);
                VMHandler.SupplierVm = this;
                ConfirmSupplierEditContentDialog cSECD = new ConfirmSupplierEditContentDialog();
                await cSECD.ShowAsync();
            }
        }

        /// <summary>
        /// Method that opens a confirmation window if you can delete a supplier
        /// </summary>
        public async void ConfirmDeleteMethod()
        {
            FeedBackText = "";
            if (CheckDeleteSupplier())
            {
                Data.SelectedSupplier = SelectedSupplier;
                VMHandler.SupplierVm = this;
                ConfirmDeleteSupplierContentDialog cDSCD = new ConfirmDeleteSupplierContentDialog();
                await cDSCD.ShowAsync();
            }
            else
            {
                FeedBackText = "Error: You cannot delete the default supplier";
            }
            
        }

        /// <summary>
        /// Method that checks if you can delete the selected supplier
        /// </summary>
        /// <returns>returns true if you can and false if you cant (supplier.id == 0)</returns>
        public bool CheckDeleteSupplier()
        {
            if (SelectedSupplier.Id == 0)
                return false;
            return true;
        }

        /// <summary>
        /// Method that shows the edit fields
        /// </summary>
        public void ShowEditMethod()
        {
            ShowNormal = false;
            ShowEdit = true;
            ShowErrorField = false;
            FeedBackText = "";
        }

        /// <summary>
        /// Creates error text for the view by using errors from CheckEditFields
        /// </summary>
        /// <param name="error">The error of enum type that is passed and used to set the text</param>
        private void SetFeedBackText(Constants.SupplierCheckErrors error)
        {
            if (error == Constants.SupplierCheckErrors.FIELDS_EMPTY)
                FeedBackText = "Error: All fields must be filled";
            if (error == Constants.SupplierCheckErrors.NAME_IN_USE)
                FeedBackText = "Error: Name in use";
            if (error == Constants.SupplierCheckErrors.OK)
                FeedBackText = "";
        }

        /// <summary>
        /// Checks the entered data for editing if, the name matches another, if the name is the same, or if all fields are filled
        /// </summary>
        /// <returns>Returns an type from an enum depending on what the fields contain</returns>
        private Constants.SupplierCheckErrors CheckEditFields()
        {
            bool expression = !string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(Address) &&
                              !string.IsNullOrEmpty(Email) && Phone != 0;
            if (expression)
            {
                if (Name.ToLower() == SelectedSupplier.Name.ToLower())
                {
                    ShowErrorField = false;
                    return Constants.SupplierCheckErrors.OK;
                }
                foreach (Supplier supplier in Data.AllSuppliers.Values)
                {
                    if (supplier.Name.ToLower() == Name.ToLower())
                    {
                        ShowErrorField = true;
                        return Constants.SupplierCheckErrors.NAME_IN_USE;
                    }
                }
                ShowErrorField = false;
                return Constants.SupplierCheckErrors.OK;
            }

            ShowErrorField = true;
            return Constants.SupplierCheckErrors.FIELDS_EMPTY;
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
