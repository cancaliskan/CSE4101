namespace araniyor.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Yorumlar")]
    public partial class Yorumlar
    {
        public int ID { get; set; }

        public int? yorumYapan { get; set; }

        public int? yorumYapilan { get; set; }

        public string yorumMetni { get; set; }

        public DateTime? yorumTarihi { get; set; }
    }
}
