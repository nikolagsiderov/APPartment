using APPartment.Data.Server.Models.Base;
using APPartment.ORM.Framework.Attributes;
using APPartment.UI.Attributes;
using System.ComponentModel.DataAnnotations.Schema;

namespace APPartment.Data.Server.Models.Objects
{
    [Table("Hygiene", Schema = "dbo")]
    public class Hygiene : HomeBaseObject
    {
        [FieldMappingForMainTable]
        [APPUIHint(Templates.Integer)]
        public int Status { get; set; }
    }
}
