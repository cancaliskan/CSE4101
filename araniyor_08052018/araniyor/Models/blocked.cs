namespace araniyor.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("blocked")]
    public partial class blocked
    {
        [Key]
        public int blockID { get; set; }
        [DisplayName("Engelleyen")]
        public int blockerID { get; set; }
        [DisplayName("Engellenen")]
        public int blockedID { get; set; }
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        [DisplayName("Engellenme Tarihi")]
        public DateTime date { get; set; }
        [DisplayName("Engelleyen")]
        public virtual Users Users { get; set; }
        [DisplayName("Engellenen")]
        public virtual Users Users1 { get; set; }
    }
}
