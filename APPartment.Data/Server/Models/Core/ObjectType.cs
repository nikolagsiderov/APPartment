using APPartment.Data.Attributes;
using System.ComponentModel.DataAnnotations.Schema;

namespace APPartment.Data.Server.Models.Core
{
    [Table("ObjectType", Schema = "dbo")]
    public class ObjectType
    {
        [FieldMappingForMainTable]
        public long Id { get; set; }

        [FieldMappingForMainTable]
        public string Name { get; set; }
    }
}
