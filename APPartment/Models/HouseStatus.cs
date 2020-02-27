using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APPartment.Models
{
    public class HouseStatus
    {
        [Key]
        public long Id { get; set; }

        public int Status { get; set; } = 1;

        public string Details { get; set; }

        [ForeignKey("User")]
        public long UserId { get; set; }

        [ForeignKey("House")]
        public long? HouseId { get; set; }
    }
}
