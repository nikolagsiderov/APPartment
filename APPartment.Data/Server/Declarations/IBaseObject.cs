using APPartment.Data.Attributes;
using APPartment.Data.Server.Models.MetaObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace APPartment.Data.Server.Declarations
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

        [NotMapped]
        public Models.Core.Object Object { get; set; }

        [NotMapped]
        public List<string> Comments { get; set; }

        [NotMapped]
        public List<Image> Images { get; set; }

        [NotMapped]
        public List<string> History { get; set; }

        [NotMapped]
        public string LastUpdated { get; set; }

        [NotMapped]
        public string LastUpdatedBy { get; set; }

        [NotMapped]
        public string LastUpdate { get; set; }
    }
}
