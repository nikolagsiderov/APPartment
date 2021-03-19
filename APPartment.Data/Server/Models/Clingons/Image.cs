using APPartment.Data.Server.Models.Base;
using APPartment.ORM.Framework.Attributes;
using System.ComponentModel.DataAnnotations.Schema;

namespace APPartment.Data.Server.Models.Clingons
{
    [Table("Image", Schema = "dbo")]
    public class Image : BaseObject
    {
        [FieldMappingForMainTable]
        public string FileSize { get; set; }

        [FieldMappingForMainTable]
        public long TargetObjectId { get; set; }
    }
}
