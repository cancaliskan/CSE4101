namespace araniyor.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Puanlar")]
    public partial class Puanlar
    {
        public int ID { get; set; }

        public int? puanVeren { get; set; }

        public int? puanVerilen { get; set; }
        public DateTime? Tarih { get; set; }
        public int Puan { get; set; }


    }
}
