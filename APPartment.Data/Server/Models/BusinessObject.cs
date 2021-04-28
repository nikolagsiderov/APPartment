using APPartment.Data.Server.Models.Base;
using APPartment.ORM.Framework.Attributes;
using System.ComponentModel.DataAnnotations.Schema;

namespace APPartment.Data.Server.Models
{
    [Table("Object", Schema = "dbo")]
    public class BusinessObject : BaseObject
    {
        [FieldMappingForObjectTable]
        public string ObjectTypeName { get; set; }

        [FieldMappingForObjectTable]
        public string Area { get; set; }
    }
}
