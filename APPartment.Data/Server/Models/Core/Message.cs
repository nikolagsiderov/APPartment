using APPartment.Data.Server.Models.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace APPartment.Data.Server.Models.Core
{
    [Table("Message", Schema = "dbo")]
    public class Message : HomeBaseObject
    {
    }
}
