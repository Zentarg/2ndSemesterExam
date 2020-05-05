using System;
using System.Runtime.CompilerServices;

namespace CommonLibrary
{
    public class Item
    {
        private int _id;
        private string _name;
        private float _price;
        private string _comment;
        private string _pictureSource;
        private int _barcode;
        private string _color;
        private string _size;
        private int _categoryId;
        private float _discountPercentage;

        public Item(int id, string name, float price)
        {
            _id = id;
            _name = name;
            _price = price;
        }

        public Item(int id, string name, float price, string comment , string pictureSource , int barcode , string color, string size, int categoryId, float discountPercentage)
        {
            _id = id;
            _name = name;
            _price = price;
            _comment = comment;
            _pictureSource = pictureSource;
            _barcode = barcode;
            _color = color;
            _size = size;
            _categoryId = categoryId;
            _discountPercentage = discountPercentage;
        }

        public int Id { get => _id; set { _id = value; } }
        public string Name { get => _name; set { _name = value; } }
        public float Price { get => _price; set { _price = value; } }
        public string Comment { get => _comment; set { _comment = value; } }
        public string PictureSource { get => _pictureSource; set { _pictureSource = value; } }
        public int Barcode { get => _barcode; set { _barcode = value; } }
        public string Color { get => _color; set { _color = value; } }
        public string Size { get => _size; set { _size = value; } }
        public int CategoryId { get => _categoryId; set { _categoryId = value; } }
        public float DiscountPercentage { get => _discountPercentage; set { _discountPercentage = value; } }

        public override string ToString()
        {
            return $"#{Id} - {Name},{Price}";
        }
    }
}
