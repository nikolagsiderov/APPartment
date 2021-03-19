using APPartment.Data.Server.Models.Base;
using APPartment.ORM.Framework.Attributes;
using System.ComponentModel.DataAnnotations.Schema;

namespace APPartment.Data.Server.Models.Clingons
{
    [Table("Comment", Schema = "dbo")]
    public class Comment : BaseObject
    {
        [FieldMappingForMainTable]
        public long TargetObjectId { get; set; }
    }
}
