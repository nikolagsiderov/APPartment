using System;

namespace APPartment.ORM.Framework.Declarations
{
    public interface IBaseObject
    {
        public long Id { get; set; }

        public long ObjectId { get; set; }

        public long ObjectTypeId { get; set; }

        public string Name { get; set; }

        public string Details { get; set; }

        public long CreatedById { get; set; }

        public DateTime CreatedDate { get; set; }

        public long? ModifiedById { get; set; }

        public DateTime? ModifiedDate { get; set; }
    }
}
