using System.ComponentModel.DataAnnotations;

namespace APPartment.Models
{
    public class HouseUser
    {
        [Key]
        public long Id { get; set; }

        public long HouseId { get; set; }

        public long UserId { get; set; }
    }
}
