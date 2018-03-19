namespace araniyor.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ProfilGoruntuleme")]
    public partial class ProfilGoruntuleme
    {
        public int ID { get; set; }

        public int? goruntuleyen { get; set; }

        public int? goruntulenen { get; set; }

        public DateTime? Tarih { get; set; }
    }
}
