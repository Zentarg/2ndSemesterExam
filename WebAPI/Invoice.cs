namespace WebAPI
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Invoice")]
    public partial class Invoice
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Invoice()
        {
            InvoiceHasItems = new HashSet<InvoiceHasItem>();
        }

        public int ID { get; set; }

        public int AuthorID { get; set; }

        public double Price { get; set; }

        public double Discount { get; set; }

        [StringLength(999)]
        public string Comment { get; set; }

        public virtual User User { get; set; }

        public int StoreID { get; set; }

        public int InvoiceStatusID { get; set; }

        [StringLength(255)]
        public string AdminComment { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InvoiceHasItem> InvoiceHasItems { get; set; }
    }
}
