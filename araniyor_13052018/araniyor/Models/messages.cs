namespace araniyor.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class messages
    {
        [Key]
        public int messageID { get; set; }
        [DisplayName("G�nderen")]
        public int senderID { get; set; }
        [DisplayName("G�nderilen")]
        public int receiverID { get; set; }

        [Required]
        [DisplayName("Mesaj")]
        public string message { get; set; }

        [DisplayName("G�nderim Tarihi")]
        public DateTime date { get; set; }

        [DisplayName("Sohbet ID")]
        public string conversationID { get; set; }
    }
}
