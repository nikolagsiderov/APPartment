using System;

namespace APPartment.ORM.Framework.Declarations
{
    public interface IBaseObject
    {
        public long ID { get; set; }

        public long ObjectID { get; set; }

        public long ObjectTypeID { get; set; }

        public string Name { get; set; }

        public string Details { get; set; }

        public long CreatedByID { get; set; }

        public DateTime CreatedDate { get; set; }

        public long? ModifiedByID { get; set; }

        public DateTime? ModifiedDate { get; set; }
    }
}
