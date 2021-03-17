using APPartment.Data.Server.Models.Base;
using APPartment.ORM.Framework.Attributes;
using APPartment.UI.Attributes;
using System.ComponentModel.DataAnnotations.Schema;

namespace APPartment.Data.Server.Models.Objects
{
    [Table("Chore", Schema = "dbo")]
    public class Chore : HomeBaseObject
    {
        [FieldMappingForMainTable]
        [APPUIHint(Templates.Integer)]
        public int Status { get; set; }

        [FieldMappingForMainTable]
        [APPUIHint(Templates.Hidden)]
        public long? AssignedToUserId { get; set; }
    }
}
