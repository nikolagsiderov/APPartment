using APPartment.Data.Attributes;
using APPartment.Data.Server.Models.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace APPartment.Data.Server.Models.Objects
{
    [Table("Issue", Schema = "dbo")]
    public class Issue : HomeBaseObject
    {
        [FieldMappingForMainTable]
        public int Status { get; set; }
    }
}
