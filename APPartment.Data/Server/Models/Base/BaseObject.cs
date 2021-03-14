using APPartment.Data.Server.Models.MetaObjects;
using APPartment.ORM.Framework.Attributes;
using APPartment.ORM.Framework.Declarations;
using System;
using System.Collections.Generic;

namespace APPartment.Data.Server.Models.Base
{
    public abstract class BaseObject : IBaseObject
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

        public List<string> Comments { get; set; }

        public List<Image> Images { get; set; }

        public string LastUpdated { get; set; }

        public string LastUpdatedBy { get; set; }

        public string LastUpdate { get; set; }
    }
}
