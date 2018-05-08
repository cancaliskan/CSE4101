namespace araniyor.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Web;

    public partial class Users
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Users()
        {
            blocked = new HashSet<blocked>();
            blocked1 = new HashSet<blocked>();

        }

        [Key]
        public int userID { get; set; }

        [DisplayName("Kategori")]
        public int? businessCategoryID { get; set; }
        [DisplayName("Alt Kategori")]
        public int? businessSubategoryID { get; set; }

        [Required]
        [StringLength(50)]
        [DisplayName("Kullan�c� Ad�")]
        public string username { get; set; }

        //[Required]
        [StringLength(50)]
        [DisplayName("Parola")]
        public string password { get; set; }

        [Required]
        [StringLength(50)]
        [EmailAddress(ErrorMessage = "Ge�ersiz E-Posta Adresi.!")]
        [DisplayName("E-Posta")]
        public string eMail { get; set; }

        [Required]
        [StringLength(50)]
        [DisplayName("�sim")]

        public string name { get; set; }

        [Required]
        [DisplayName("Soyisim")]
        [StringLength(50)]
        public string surname { get; set; }

        [DisplayName("Hizmet veren mi?")]
        public bool steward { get; set; }


        [DisplayName("Resim")]
        [Column(TypeName = "image")]
        public byte[] picture { get; set; }

        [DisplayName("�lke")]
        [StringLength(20)]
        public string country { get; set; }

        [DisplayName("�ehir")]
        [StringLength(20)]
        public string city { get; set; }

        [DisplayName("�l�e")]
        [StringLength(20)]
        public string district { get; set; }
        [DisplayName("Do�um Tarihi")]
        public DateTime? birthday { get; set; }

        [StringLength(13,MinimumLength = 10, ErrorMessage = "En az 10 haneli numara girilmeli.")]
        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "Sadece say� girilebilir.")]
        [DisplayName("Telefon")]
        public string phone { get; set; }
        [DisplayName("A��klama")]
        public string description { get; set; }
        public int? votingNumber { get; set; }

        public int? totalScore { get; set; }
        [DisplayName("Puan")]
        public double? score { get; set; }

        [DisplayName("Yorum Say�s�")]
        public int? numberOfComments { get; set; }

        [DisplayName("G�r�nt�lenme Say�s�")]
        public int? numberOfViews { get; set; }

        [DisplayName("Aktif mi?")]
        public bool active { get; set; }

        [DisplayName("Cinsiyet")]
        [StringLength(5)]
        public string gender { get; set; }
        [DisplayName("Deneyim")]
        public int? experience { get; set; }

        [DisplayName("Admin mi?")]
        public bool admin { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<blocked> blocked { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<blocked> blocked1 { get; set; }

        public virtual businessCategory businessCategory { get; set; }

        public virtual businessSubategory businessSubategory { get; set; }

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<messages> messages { get; set; }

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<messages> messages1 { get; set; }
    }
}
