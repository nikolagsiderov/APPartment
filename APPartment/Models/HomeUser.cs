using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APPartment.Models
{
    public class HomeUser
    {
        [Key]
        public long Id { get; set; }

        [ForeignKey("Home")]
        public long HomeId { get; set; }

        [ForeignKey("User")]
        public long UserId { get; set; }
    }
}
