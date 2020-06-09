namespace WebAPI
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Log")]
    public partial class Log
    {
        public int ID { get; set; }

        public int UserID { get; set; }

        [Required]
        [StringLength(255)]
        public string LogEntry { get; set; }

        public DateTime DateAndTime { get; set; }

        public int RequestType { get; set; }
    }
}
