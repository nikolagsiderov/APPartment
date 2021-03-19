using APPartment.Data.Server.Models.Base;
using APPartment.ORM.Framework.Attributes;
using System.ComponentModel.DataAnnotations.Schema;

namespace APPartment.Data.Server.Models.Home
{
    [Table("Home", Schema = "dbo")]
    public class Home : BaseObject
    {
        [FieldMappingForMainTable]
        public string Password { get; set; }
    }
}
