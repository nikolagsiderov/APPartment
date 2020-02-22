using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APPartment.Models
{
    public class HouseSettings
    {
        [Key]
        public long Id { get; set; }

        public int RentDueDateDay { get; set; }

        public string HouseName { get; set; }

        [ForeignKey("House")]
        public long? HouseId { get; set; }
    }
}
