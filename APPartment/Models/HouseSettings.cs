using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APPartment.Models
{
    public class HouseSettings
    {
        [Key]
        public long Id { get; set; }

        [DataType(DataType.Date)]
        public DateTime RentDueDate { get; set; }

        [ForeignKey("House")]
        public long? HouseId { get; set; }
    }
}
