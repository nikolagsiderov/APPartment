using APPartment.Data.Server.Models.Base;
using APPartment.ORM.Framework.Attributes;
using System.ComponentModel.DataAnnotations.Schema;

namespace APPartment.Data.Server.Models.Hygiene
{
    [Table("Hygiene", Schema = "dbo")]
    public class Hygiene : HomeBaseObject
    {
        [FieldMappingForMainTable]
        public bool IsDone { get; set; }
    }
}
