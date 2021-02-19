using APPartment.Data.Attributes;
using APPartment.Data.Server.Models.Base;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace APPartment.Data.Server.Models.Core
{
    [Obsolete]
    [Table("Audit", Schema = "dbo")]
    public class Audit : HomeBaseObject
    {
        [FieldMappingForMainTable]
        public string TableName { get; set; }

        [FieldMappingForMainTable]
        public DateTime When { get; set; }

        [FieldMappingForMainTable]
        public string KeyValues { get; set; }

        [FieldMappingForMainTable]
        public string OldValues { get; set; }

        [FieldMappingForMainTable]
        public string NewValues { get; set; }

        [FieldMappingForMainTable]
        public long UserId { get; set; }

        [FieldMappingForMainTable]
        public long? TargetObjectId { get; set; }
    }
}
