using APPartment.Data.Server.Models.Base;
using APPartment.ORM.Framework.Attributes;
using System.ComponentModel.DataAnnotations.Schema;

namespace APPartment.Data.Server.Models.Notification
{
    [Table("NotificationParticipant", Schema = "dbo")]
    public class NotificationParticipant : BaseObject
    {
        [FieldMappingForMainTable]
        public long NotificationID { get; set; }

        [FieldMappingForMainTable]
        public long UserID { get; set; }
    }
}
