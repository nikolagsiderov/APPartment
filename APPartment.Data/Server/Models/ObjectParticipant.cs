using APPartment.Data.Server.Models.Base;
using APPartment.ORM.Framework.Attributes;
using System.ComponentModel.DataAnnotations.Schema;

namespace APPartment.Data.Server.Models
{
    [Table("ObjectParticipant", Schema = "dbo")]
    public class ObjectParticipant : BaseObject
    {
        [FieldMappingForMainTable]
        public long TargetObjectID { get; set; }

        [FieldMappingForMainTable]
        public long UserID { get; set; }
    }
}
