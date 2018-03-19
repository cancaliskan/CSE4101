namespace araniyor.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class messages
    {
        [Key]
        public int messageID { get; set; }

        public int senderID { get; set; }

        public int receiverID { get; set; }

        [Required]
        public string message { get; set; }

        public DateTime date { get; set; }

        public string conversationID { get; set; }
    }
}
