namespace WebAPI
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Salary")]
    public partial class Salary
    {
        [Key]
        public int UserID { get; set; }

        public double BeforeTax { get; set; }

        public double TaxPercentage { get; set; }

        public virtual User User { get; set; }
    }
}
