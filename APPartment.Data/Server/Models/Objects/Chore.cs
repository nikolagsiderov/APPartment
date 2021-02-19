using APPartment.Data.Attributes;
using APPartment.Data.Server.Models.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace APPartment.Data.Server.Models.Objects
{
    [Table("Chore", Schema = "dbo")]
    public class Chore : HomeBaseObject
    {
        [FieldMappingForMainTable]
        public int Status { get; set; }

        [FieldMappingForMainTable]
        public long? AssignedToUserId { get; set; }
    }
}
