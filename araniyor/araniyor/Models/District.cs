namespace araniyor.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("District")]
    public partial class District
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }

        [Column("District")]
        [StringLength(20)]
        public string District1 { get; set; }

        public int? CityID { get; set; }

        public virtual City City { get; set; }
    }
}
