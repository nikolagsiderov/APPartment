using APPartment.Data.Attributes;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace APPartment.Data.Server.Models.Core
{
    [Obsolete]
    [Table("Object", Schema = "dbo")]
    public class Object
    {
        [FieldMappingForObjectTable]
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
        public long ModifiedById { get; set; }

        [FieldMappingForObjectTable]
        public DateTime ModifiedDate { get; set; }
    }
}
