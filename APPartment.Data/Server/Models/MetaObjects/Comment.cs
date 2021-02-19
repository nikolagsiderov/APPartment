using APPartment.Data.Attributes;
using APPartment.Data.Server.Models.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace APPartment.Data.Server.Models.MetaObjects
{
    [Table("Comment", Schema = "dbo")]
    public class Comment : IdentityBaseObject
    {
        [FieldMappingForMainTable]
        public long TargetObjectId { get; set; }
    }
}
