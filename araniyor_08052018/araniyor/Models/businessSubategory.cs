namespace araniyor.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("businessSubategory")]
    public partial class businessSubategory
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public businessSubategory()
        {
            Users = new HashSet<Users>();
        }
        [DisplayName("Alt Kategori")]
        public int businessSubategoryID { get; set; }
        [DisplayName("Kategori")]
        public int businessCategoryID { get; set; }

        [Column("businessSubategory")]
        [Required]
        [StringLength(50)]
        [DisplayName("Alt Kategori")]
        public string businessSubategory1 { get; set; }
        [DisplayName("Kategori")]
        public virtual businessCategory businessCategory { get; set; }
        [DisplayName("Kategori")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Users> Users { get; set; }
    }
}
