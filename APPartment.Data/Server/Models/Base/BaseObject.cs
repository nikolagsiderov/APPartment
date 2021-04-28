using APPartment.ORM.Framework.Attributes;
using APPartment.ORM.Framework.Declarations;
using System;

namespace APPartment.Data.Server.Models.Base
{
    public abstract class BaseObject : IBaseObject
    {
        [FieldMappingForMainTablePrimaryKey]
        public long ID { get; set; }

        [FieldMappingForObjectTablePrimaryKey]
        public long ObjectID { get; set; }

        [FieldMappingForObjectTable]
        public long ObjectTypeID { get; set; }

        [FieldMappingForObjectTable]
        public long? HomeID { get; set; }

        [FieldMappingForObjectTable]
        public string Name { get; set; }

        [FieldMappingForObjectTable]
        public string Details { get; set; }

        [FieldMappingForObjectTable]
        public long CreatedByID { get; set; }

        [FieldMappingForObjectTable]
        public DateTime CreatedDate { get; set; }

        [FieldMappingForObjectTable]
        public long? ModifiedByID { get; set; }

        [FieldMappingForObjectTable]
        public DateTime? ModifiedDate { get; set; }

        [FieldMappingForObjectTable]
        public long MainID { get; set; }
    }
}
