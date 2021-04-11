using APPartment.Data.Server.Models.Base;
using APPartment.ORM.Framework.Attributes;
using System.ComponentModel.DataAnnotations.Schema;

namespace APPartment.Data.Server.Models.Inventory
{
    [Table("Inventory", Schema = "dbo")]
    public class Inventory : BaseObject
    {
        [FieldMappingForMainTable]
        public bool IsSupplied { get; set; }
    }
}
