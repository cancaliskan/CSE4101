namespace araniyor.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Web.Mvc;

    public partial class AboutUs
    {
        public int ID { get; set; }

        [Required]
        [DisplayName("Hakkimizda Metni")]
        public string Hakkimizda { get; set; }

        [DisplayName("Misyonumuz Metni")]
        [Required]
        public string Misyon { get; set; }

        [DisplayName("Vizyonumuz Metni")]
        [Required]
        public string Vizyon { get; set; }

        [Required]
        [DisplayName("Adres Bilgisi")]
        [AllowHtml]
        public string Adres { get; set; }

        [Required]
        [StringLength(15, MinimumLength = 10, ErrorMessage = "En az 10 haneli numara girilmeli.")]
        [DisplayName("Telefon Numarası")]
        public string Telefon { get; set; }

        [Required]
        [DisplayName("E-Posta Adresi")]
        [StringLength(50)]
        [EmailAddress(ErrorMessage = "Geçersiz E-Posta Adresi.!")]
        public string EPosta { get; set; }

        [DisplayName("Facebook Adresi")]
        [StringLength(100)]
        public string Facebook { get; set; }

        [DisplayName("Twitter Adresi")]
        [StringLength(100)]
        public string Twitter { get; set; }

        [DisplayName("LinkedIn Adresi")]
        [StringLength(100)]
        public string LinkedIn { get; set; }

       
        [DisplayName("Hakkımızda Resmi")]
        [Column(TypeName = "image")]
        public byte[] HakkimizdaResim { get; set; }

        [DisplayName("Vizyon Resmi")]
        [Column(TypeName = "image")]
        public byte[] VizyonResim { get; set; }

        [DisplayName("Misyon Resmi")]
        [Column(TypeName = "image")]
        public byte[] MisyonResim { get; set; }

        [DisplayName("Slider Resim 1")]
        [Column(TypeName = "image")]
        public byte[] SliderResim1 { get; set; }

        [DisplayName("Slider Resim 2")]
        [Column(TypeName = "image")]
        public byte[] SliderResim2 { get; set; }

        [DisplayName("Slider Resim 3")]
        [Column(TypeName = "image")]
        public byte[] SliderResim3 { get; set; }


        [DisplayName("Slider Resim 4")]
        [Column(TypeName = "image")]
        public byte[] SliderResim4 { get; set; }

        [Required]
        [DisplayName("Kullanıcı Sözleşmesi")]
        public string KullaniciSozlesmesi { get; set; }



    }
}
