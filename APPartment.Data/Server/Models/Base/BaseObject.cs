using APPartment.Data.Server.Models.MetaObjects;
using APPartment.ORM.Framework.Attributes;
using APPartment.ORM.Framework.Declarations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace APPartment.Data.Server.Models.Base
{
    public abstract class BaseObject : IBaseObject
    {
        [FieldMappingForObjectTablePrimaryKey]
        public long ObjectId { get; set; }

        [FieldMappingForObjectTable]
        [NotMapped]
        public long ObjectTypeId { get; set; }

        [FieldMappingForObjectTable]
        [NotMapped]
        public string Name { get; set; }

        [FieldMappingForObjectTable]
        [NotMapped]
        public string Details { get; set; }

        [FieldMappingForObjectTable]
        [NotMapped]
        public long CreatedById { get; set; }

        [FieldMappingForObjectTable]
        [NotMapped]
        public DateTime CreatedDate { get; set; }

        [FieldMappingForObjectTable]
        [NotMapped]
        public long? ModifiedById { get; set; }

        [FieldMappingForObjectTable]
        [NotMapped]
        public DateTime? ModifiedDate { get; set; }

        [NotMapped]
        public List<string> Comments { get; set; }

        [NotMapped]
        public List<Image> Images { get; set; }

        [NotMapped]
        public string LastUpdated { get; set; }

        [NotMapped]
        public string LastUpdatedBy { get; set; }

        [NotMapped]
        public string LastUpdate { get; set; }
    }
}
