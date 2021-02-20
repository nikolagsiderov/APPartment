using APPartment.Data.Server.Models.Base;
using APPartment.ORM.Framework.Attributes;
using System.ComponentModel.DataAnnotations.Schema;

namespace APPartment.Data.Server.Models.Objects
{
    [Table("Inventory", Schema = "dbo")]
    public class Inventory : HomeBaseObject
    {
        [FieldMappingForMainTable]
        public int Status { get; set; }
    }
}
