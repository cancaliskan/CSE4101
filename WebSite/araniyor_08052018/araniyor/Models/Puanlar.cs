namespace araniyor.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Puanlar")]
    public partial class Puanlar
    {
        public int ID { get; set; }
        [DisplayName("Puan Veren")]
        public int? puanVeren { get; set; }
        [DisplayName("Puan Verilen")]
        public int? puanVerilen { get; set; }

        [DisplayName("Tarih")]
        public DateTime? Tarih { get; set; }
        [DisplayName("Puan")]
        public int Puan { get; set; }


    }
}
