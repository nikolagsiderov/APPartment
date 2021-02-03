using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APPartment.Data.Server.Models.Core
{
    public class HomeSettings : Base.Object
    {
        [Key]
        public long Id { get; set; }

        public int RentDueDateDay { get; set; }

        public string HomeName { get; set; }

        [ForeignKey("Home")]
        public long? HomeId { get; set; }
    }
}
