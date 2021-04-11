using APPartment.Data.Server.Models.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace APPartment.Data.Server.Models.Notification
{
    [Table("Notification", Schema = "dbo")]
    public class Notification : BaseObject
    {
    }
}
