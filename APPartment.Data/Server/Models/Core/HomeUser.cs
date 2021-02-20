using APPartment.Data.Server.Models.Base;
using APPartment.ORM.Framework.Attributes;
using System.ComponentModel.DataAnnotations.Schema;

namespace APPartment.Data.Server.Models.Core
{
    [Table("HomeUser", Schema = "dbo")]
    public class HomeUser : HomeBaseObject
    {
        [FieldMappingForMainTable]
        public long UserId { get; set; }
    }
}
