using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace APPartment.Models
{
    public class House
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Details { get; set; }

        public int Status { get; set; }

        [Required]
        public string CreatedBy { get; set; }

        public string ModifiedBy { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        public DateTime ModifiedDate { get; set; }
    }
}
