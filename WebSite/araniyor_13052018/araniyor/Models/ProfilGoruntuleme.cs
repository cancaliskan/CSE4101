namespace araniyor.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ProfilGoruntuleme")]
    public partial class ProfilGoruntuleme
    {
        public int ID { get; set; }

        [DisplayName("Görüntüleyen")]
        public int? goruntuleyen { get; set; }

        [DisplayName("Görüntülenen")]
        public int? goruntulenen { get; set; }

        [DisplayName("Görüntülenme Tarihi")]
        public DateTime? Tarih { get; set; }
    }
}
