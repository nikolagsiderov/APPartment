using APPartment.Data.Server.Models.Base;
using APPartment.ORM.Framework.Attributes;
using APPartment.UI.Attributes;
using System.ComponentModel.DataAnnotations.Schema;

namespace APPartment.Data.Server.Models.Objects
{
    [Table("Survey", Schema = "dbo")]
    public class Survey : HomeBaseObject
    {
        [FieldMappingForMainTable]
        [APPUIHint(Templates.Integer)]
        public int Status { get; set; }

        [FieldMappingForMainTable]
        [APPUIHint(Templates.Boolean)]
        public bool IsCompleted { get; set; } = false;
    }
}
