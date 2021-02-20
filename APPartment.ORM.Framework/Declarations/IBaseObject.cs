using APPartment.ORM.Framework.Attributes;
using System;

namespace APPartment.ORM.Framework.Declarations
{
    public interface IBaseObject
    {
        [FieldMappingForObjectTablePrimaryKey]
        public long ObjectId { get; set; }

        [FieldMappingForObjectTable]
        public long ObjectTypeId { get; set; }

        [FieldMappingForObjectTable]
        public string Name { get; set; }

        [FieldMappingForObjectTable]
        public string Details { get; set; }

        [FieldMappingForObjectTable]
        public long CreatedById { get; set; }

        [FieldMappingForObjectTable]
        public DateTime CreatedDate { get; set; }

        [FieldMappingForObjectTable]
        public long? ModifiedById { get; set; }

        [FieldMappingForObjectTable]
        public DateTime? ModifiedDate { get; set; }
    }
}
