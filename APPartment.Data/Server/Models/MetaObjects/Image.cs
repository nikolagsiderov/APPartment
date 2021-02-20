using APPartment.Data.Server.Models.Base;
using APPartment.ORM.Framework.Attributes;
using System.ComponentModel.DataAnnotations.Schema;

namespace APPartment.Data.Server.Models.MetaObjects
{
    [Table("Image", Schema = "dbo")]
    public class Image : IdentityBaseObject
    {
        [FieldMappingForMainTable]
        public string FileSize { get; set; }

        [FieldMappingForMainTable]
        public long TargetObjectId { get; set; }
    }
}
