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
    public class AddItemViewModel : INotifyPropertyChanged
    {
        private string _name;
        private float _price;
        private int _discount;
        private Category _category;
        private int _categoryID;
        private string _color;
        private string _size;
        private string _comment; 
        private string _pictureUrl = "https://lh3.googleusercontent.com/proxy/H7RfYt-nkhjZ4iyB1bGL4gbUbG5rizq9dEMHT7V3_LB8CxT1kAnGIJf-eBzFemJdl7VzVlh_9ZhYZtoYidLd393lUIFvjmKuOjag2WziBrDZrKDCMr8YzNNI1CKWKpD_xBSKhWA";
        private int _barcode;
        private string _errorMessage;

        public AddItemViewModel()
        {
            LoadDataAsync();
            VMHandler.AddItemViewModel = this;
            AddItemCommand = new RelayCommand(AddItem);
            CancelCommand = new RelayCommand(NavigateBack);
        }

        public RelayCommand CancelCommand { get; set; }


        public ObservableCollection<Category> Categories
        {
            get => new ObservableCollection<Category>(Data.AllCategories.Values);
        }

        public ObservableCollection<Item> Items { get => new ObservableCollection<Item>(Data.AllItems.Values);}

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

        public Category Category
        {
            get => _category;
            set { _category = value; OnPropertyChanged(); }
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
        public RelayCommand AddItemCommand { get; }

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

        public bool CheckTextFields()
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

        public async void LoadDataAsync()
        { 
            await Data.UpdateCategories();
            await Data.UpdateItems();
            OnPropertyChanged(nameof(Categories));
            OnPropertyChanged(nameof(Items));

        }

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
