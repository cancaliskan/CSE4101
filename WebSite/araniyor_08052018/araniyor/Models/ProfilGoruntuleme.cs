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

        [DisplayName("G�r�nt�leyen")]
        public int? goruntuleyen { get; set; }

        [DisplayName("G�r�nt�lenen")]
        public int? goruntulenen { get; set; }

        [DisplayName("G�r�nt�lenme Tarihi")]
        public DateTime? Tarih { get; set; }
    }
}
