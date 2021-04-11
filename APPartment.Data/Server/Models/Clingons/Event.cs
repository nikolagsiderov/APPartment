using APPartment.Data.Server.Models.Base;
using APPartment.ORM.Framework.Attributes;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace APPartment.Data.Server.Models.Clingons
{
    [Table("Event", Schema = "dbo")]
    public class Event : BaseObject
    {
        [FieldMappingForMainTable]
        public DateTime StartDate { get; set; }

        [FieldMappingForMainTable]
        public DateTime EndDate { get; set; }

        [FieldMappingForMainTable]
        public long TargetObjectID { get; set; }
    }
}
