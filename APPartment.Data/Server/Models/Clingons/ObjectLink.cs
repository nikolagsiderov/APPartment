using APPartment.Data.Server.Models.Base;
using APPartment.ORM.Framework.Attributes;
using System.ComponentModel.DataAnnotations.Schema;

namespace APPartment.Data.Server.Models.Clingons
{
    [Table("ObjectLink", Schema = "dbo")]
    public class ObjectLink : BaseObject
    {
        [FieldMappingForMainTable]
        public long ObjectBID { get; set; }

        [FieldMappingForMainTable]
        public string ObjectLinkType { get; set; }

        [FieldMappingForMainTable]
        public long TargetObjectID { get; set; }
    }
}
