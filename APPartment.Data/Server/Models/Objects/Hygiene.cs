using APPartment.Data.Attributes;
using APPartment.Data.Server.Models.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace APPartment.Data.Server.Models.Objects
{
    [Table("Hygiene", Schema = "dbo")]
    public class Hygiene : HomeBaseObject
    {
        [FieldMappingForMainTable]
        public int Status { get; set; }
    }
}
