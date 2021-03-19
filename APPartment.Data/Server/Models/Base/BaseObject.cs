using APPartment.ORM.Framework.Attributes;
using APPartment.ORM.Framework.Declarations;
using System;

namespace APPartment.Data.Server.Models.Base
{
    public abstract class BaseObject : IBaseObject
    {
        [FieldMappingForMainTablePrimaryKey]
        public long Id { get; set; }

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
