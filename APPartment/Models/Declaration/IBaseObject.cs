using System;
using System.Collections.Generic;

namespace APPartment.Models.Declaration
{
    public interface IBaseObject
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Details { get; set; }

        public int Status { get; set; }

        public string CreatedBy { get; set; }

        public string ModifiedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public long? HouseId { get; set; }

        public long ObjectId { get; set; }

        public List<string> Comments { get; set; }
    }
}
