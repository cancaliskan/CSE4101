namespace araniyor.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Yorumlar")]
    public partial class Yorumlar
    {
        public int ID { get; set; }

        [DisplayName("Yorum Yapan")]
        public int? yorumYapan { get; set; }

        [DisplayName("Yorum Yapýlan")]
        public int? yorumYapilan { get; set; }


        [DisplayName("Yorum")]
        public string yorumMetni { get; set; }

        [DisplayName("Yorum Tarihi")]
        public DateTime? yorumTarihi { get; set; }
    }
}
