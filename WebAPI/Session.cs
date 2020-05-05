namespace WebAPI
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Session")]
    public partial class Session
    {
        [Key]
        [StringLength(32)]
        public string SessionKey { get; set; }

        public int UserID { get; set; }
    }
}
