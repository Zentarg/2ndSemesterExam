namespace WebAPI
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("User")]
    public partial class User
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public User()
        {
            Invoices = new HashSet<Invoice>();
            Stores = new HashSet<Store>();
            Stores1 = new HashSet<Store>();
        }

        public int ID { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        [Required]
        [StringLength(255)]
        public string Email { get; set; }

        public int Phone { get; set; }

        [Required]
        [StringLength(255)]
        public string Address { get; set; }

        public int RoleID { get; set; }

        public int TAJNumber { get; set; }

        public int TAXNumber { get; set; }

        public double WorkingHours { get; set; }

        public int UserLevelID { get; set; }

        public int? StoreID { get; set; }

        public virtual Auth Auth { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Invoice> Invoices { get; set; }

        public virtual Role Role { get; set; }

        public virtual Salary Salary { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Store> Stores { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Store> Stores1 { get; set; }

        public virtual Store Store { get; set; }

        public virtual UserLevel UserLevel { get; set; }
    }
}
