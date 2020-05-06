using System;
using System.Collections.Generic;
using System.Text;

namespace CommonLibrary.Models
{
    public class Salary
    {
        public Salary()
        {

        }

        public Salary(int userId, float beforeTax, float taxPercentage)
        {
            UserId = userId;
            BeforeTax = beforeTax;
            TaxPercentage = taxPercentage;
        }

        public int UserId { get; set; }
        public float BeforeTax { get; set; }
        public float TaxPercentage { get; set; }
    }
}
