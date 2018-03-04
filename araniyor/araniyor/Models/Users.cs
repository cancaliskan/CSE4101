namespace araniyor.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Users
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Users()
        {
            blocked = new HashSet<blocked>();
            blocked1 = new HashSet<blocked>();
            messages = new HashSet<messages>();
            messages1 = new HashSet<messages>();
        }

        [Key]
        public int userID { get; set; }

        public int? businessCategoryID { get; set; }

        public int? businessSubategoryID { get; set; }

        [Required]
        [StringLength(50)]
        public string username { get; set; }

        [Required]
        [StringLength(50)]
        public string password { get; set; }

        [Required]
        [StringLength(50)]
        public string eMail { get; set; }

        [Required]
        [StringLength(50)]
        public string name { get; set; }

        [Required]
        [StringLength(50)]
        public string surname { get; set; }

        public bool steward { get; set; }

        [Column(TypeName = "image")]
        public byte[] picture { get; set; }

        [StringLength(20)]
        public string country { get; set; }

        [StringLength(20)]
        public string city { get; set; }

        [StringLength(20)]
        public string district { get; set; }

        public DateTime? birthday { get; set; }

        [StringLength(13)]
        public string phone { get; set; }

        public string description { get; set; }

        public int? votingNumber { get; set; }

        public int? totalScore { get; set; }

        public double? score { get; set; }

        public int? numberOfComments { get; set; }

        public int? numberOfViews { get; set; }

        public bool active { get; set; }

        [StringLength(10)]
        public string gender { get; set; }

        public int? experience { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<blocked> blocked { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<blocked> blocked1 { get; set; }

        public virtual businessCategory businessCategory { get; set; }

        public virtual businessSubategory businessSubategory { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<messages> messages { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<messages> messages1 { get; set; }
    }
}
