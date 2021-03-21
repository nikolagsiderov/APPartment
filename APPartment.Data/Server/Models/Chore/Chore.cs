using APPartment.Data.Server.Models.Base;
using APPartment.ORM.Framework.Attributes;
using System.ComponentModel.DataAnnotations.Schema;

namespace APPartment.Data.Server.Models.Chore
{
    [Table("Chore", Schema = "dbo")]
    public class Chore : HomeBaseObject
    {
        [FieldMappingForMainTable]
        public long? AssignedToUserId { get; set; }
        public bool IsDone { get; set; }
    }
}
