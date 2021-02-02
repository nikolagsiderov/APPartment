using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APPartment.Data.Models.Core
{
    public class HomeStatus : Base.Object
    {
        [Key]
        public long Id { get; set; }

        public int Status { get; set; } = 1;

        public string Details { get; set; }

        [ForeignKey("User")]
        public long UserId { get; set; }

        [ForeignKey("Home")]
        public long? HomeId { get; set; }
    }
}
