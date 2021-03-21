using APPartment.Data.Server.Models.Base;
using APPartment.ORM.Framework.Attributes;
using System.ComponentModel.DataAnnotations.Schema;

namespace APPartment.Data.Server.Models.Issue
{
    [Table("Issue", Schema = "dbo")]
    public class Issue : HomeBaseObject
    {
        [FieldMappingForMainTable]
        public bool IsClosed { get; set; }
    }
}
