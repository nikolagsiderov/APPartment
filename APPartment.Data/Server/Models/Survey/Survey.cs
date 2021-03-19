using APPartment.Data.Server.Models.Base;
using APPartment.ORM.Framework.Attributes;
using System.ComponentModel.DataAnnotations.Schema;

namespace APPartment.Data.Server.Models.Survey
{
    [Table("Survey", Schema = "dbo")]
    public class Survey : HomeBaseObject
    {
        [FieldMappingForMainTable]
        public bool IsCompleted { get; set; } = false;
    }
}
