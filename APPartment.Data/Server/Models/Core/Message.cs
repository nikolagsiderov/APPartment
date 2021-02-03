using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APPartment.Data.Server.Models.Core
{
    public class Message : Base.Object
    {
        [Key]
        public long Id { get; set; }

        public string Username { get; set; }

        public string Text { get; set; }

        public DateTime CreatedDate { get; set; }

        [ForeignKey("User")]
        public long UserId { get; set; }

        [ForeignKey("Home")]
        public long HomeId { get; set; }
    }
}
