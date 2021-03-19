using APPartment.Data.Server.Models.Base;
using APPartment.ORM.Framework.Attributes;
using System.ComponentModel.DataAnnotations.Schema;

namespace APPartment.Data.Server.Models.User
{
    [Table("User", Schema = "dbo")]
    public class User : BaseObject
    {
        [FieldMappingForMainTable]
        public string Password { get; set; }
    }
}
