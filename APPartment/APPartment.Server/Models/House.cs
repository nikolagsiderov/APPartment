using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APPartment.Server.Models
{
    public class House
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Details { get; set; }

        public string Status { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
