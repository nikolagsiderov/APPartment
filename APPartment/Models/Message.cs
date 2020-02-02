using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APPartment.Models
{
    public class Message
    {
        [Key]
        public long Id { get; set; }

        public string Username { get; set; }

        public string Text { get; set; }

        public DateTime CreatedDate { get; set; }

        [ForeignKey("User")]
        public long UserId { get; set; }
    }
}
