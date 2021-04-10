using APPartment.Data.Server.Models.Base;
using APPartment.ORM.Framework.Attributes;
using System.ComponentModel.DataAnnotations.Schema;

namespace APPartment.Data.Server.Models.Clingons
{
    [Table("EventParticipant", Schema = "dbo")]
    public class EventParticipant : BaseObject
    {
        [FieldMappingForMainTable]
        public long EventID { get; set; }

        [FieldMappingForMainTable]
        public long UserID { get; set; }
    }
}
