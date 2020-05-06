using System;
using System.Runtime.CompilerServices;

namespace CommonLibrary.Models
{
    public class Item
    {


        public Item(int id, string name, float price, string comment , string pictureSource , int barcode , string color, string size, int categoryId, float discountPercentage)
        {
            Id = id;
            Name = name;
            Price = price;
            Comment = comment;
            PictureSource = pictureSource;
            Barcode = barcode;
            Color = color;
            Size = size;
            CategoryId = categoryId;
            DiscountPercentage = discountPercentage;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public float Price { get; set; }
        public string Comment { get; set; } = "";
        public string PictureSource { get; set; } = "";
        public int Barcode { get; set; } = 0;
        public string Color { get; set; } = "";
        public string Size { get; set; } = "";
        public int CategoryId { get; set; }
        public float DiscountPercentage { get; set; } = 0;

        public override string ToString()
        {
            return $"#{Id} - {Name},{Price}";
        }
    }
}
