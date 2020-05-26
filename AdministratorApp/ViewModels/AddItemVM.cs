using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.UserDataTasks;
using Windows.UI.Xaml.Controls;
using AdministratorApp.Annotations;
using AdministratorApp.Models;
using CommonLibrary.Models;
using GalaSoft.MvvmLight.Command;

namespace AdministratorApp.ViewModels
{
    public class AddItemVM : INotifyPropertyChanged
    {
        //Instance fields 
        private string _name;
        private float _price;
        private int _discount;
        private Category _category;
        private int _categoryID;
        private string _color;
        private string _size;
        private string _comment; 
        private string _pictureUrl = "https://image.flaticon.com/icons/svg/809/809130.svg";
        private int _barcode;
        private string _errorMessage;


        //Constructor for setting up the commands, loading data from the database using Data.cs and sending the ViewModel to VMHandler
        public AddItemVM()
        {
            LoadDataAsync();
            VMHandler.AddItemViewModel = this;
            AddItemCommand = new RelayCommand(AddItem);
            CancelCommand = new RelayCommand(NavigateBack);
        }

        //Relay commands for addingItem and cancelling action.
        public RelayCommand AddItemCommand { get; }
        public RelayCommand CancelCommand { get; set; }

        //Observable collection for the list of categories, returns the values from the Data class

        public ObservableCollection<Category> Categories
        {
            get => new ObservableCollection<Category>(Data.AllCategories.Values);
        }

        public ObservableCollection<Item> Items { get => new ObservableCollection<Item>(Data.AllItems.Values);}

        //Reference type property for Category

        public Category Category
        {
            get => _category;
            set { _category = value; OnPropertyChanged(); }
        }

        //Value type properties

        public string Name { get => _name;
            set{ _name = value; OnPropertyChanged();} }
        public float Price
        {
            get => _price;
            set { _price = value; OnPropertyChanged(); }
        }
        public int Discount
        {
            get => _discount;
            set { _discount = value; OnPropertyChanged(); }
        }

        public int CategoryID
        {
            get => _categoryID;
            set { _categoryID = value; OnPropertyChanged(); }
        }
        public string Color
        {
            get => _color;
            set { _color = value; OnPropertyChanged(); }
        }
        public string Size
        {
            get => _size;
            set { _size = value; OnPropertyChanged(); }
        }
        public string Comment
        {
            get => _comment;
            set { _comment = value; OnPropertyChanged(); }
        }
        public string PictureUrl
        {
            get => _pictureUrl;
            set { _pictureUrl = value; OnPropertyChanged(); }
        }
        public int Barcode
        {
            get => _barcode;
            set { _barcode = value; OnPropertyChanged(); }
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set { _errorMessage = value; OnPropertyChanged(); }
        }


       /// <summary>
       /// Method for creating new item and adding it to the database trough the API handler.
       /// </summary>
        
        private async void AddItem()
        {

            try
            {
                if (CheckTextFields())
                {
                    if (Price > 0)
                    {

                        if (Discount <= 100)
                        {
                            if (CheckIfNameAlreadyExist())
                            {


                                Item newItem = new Item(Name, Price, Comment, PictureUrl, Barcode, Color, Size,
                                    CategoryID,
                                    Discount);
                                await APIHandler<Item>.PostOne("Items", newItem);
                                ContentDialog dialog = new ContentDialog()
                                {
                                    Title = "Item successfully added!",
                                    Content = $"{Name} was successfully added to the database!",
                                    PrimaryButtonText = "Ok"
                                };

                                await dialog.ShowAsync();
                                await Data.UpdateItems();
                                OnPropertyChanged(nameof(Items));
                            }
                            else ErrorMessage = $"Item with the name {Name} already exist!";


                        }
                        else ErrorMessage = "Discount cannot be more than 100%!";
                    }
                    else ErrorMessage = "Price cannot be zero!";
                }
                else ErrorMessage = "Name, category, color, size, price and barcode must be given!";

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }

       /// <summary>
       /// Method for checking if an item with the given name has already been registered to the system.
       /// </summary>
       /// <returns></returns>
        public bool CheckIfNameAlreadyExist()
        {
            foreach (var item in Items)
            {
                if (item.Name == Name)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        ///  Method for checking if all required textbox is filled out.
        /// </summary>
        /// <returns> It returns a true or false value depending on if each field is filled out.</returns>
        private bool CheckTextFields()
        {
            bool expression = (!string.IsNullOrEmpty(Name) &&
                               Category!=null && !string.IsNullOrEmpty(Color) &&
                               !string.IsNullOrEmpty(Size) &&
                               Price != 0 && Barcode != 0);
            if (expression)
            {
                return true;
            }

            return false;
        }


        /// <summary>
        /// Method to load relevant data from the database using Data.cs, and updating properties Categories, and Items.
        /// </summary>
        public async void LoadDataAsync()
        { 
            await Data.UpdateCategories();
            await Data.UpdateItems();
            OnPropertyChanged(nameof(Categories));
            OnPropertyChanged(nameof(Items));

        }

        /// <summary>
        /// Method for navigating back to the previous page.
        /// </summary>
        private void NavigateBack()
        {
            NavigationHandler.NavigateBackwards();
        }


        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


    }
}
