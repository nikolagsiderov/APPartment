using APPartment.Data.Attributes;
using APPartment.Data.Server.Models.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace APPartment.Data.Server.Models.Core
{
    [Table("HomeStatus", Schema = "dbo")]
    public class HomeStatus : HomeBaseObject
    {
        [FieldMappingForMainTable]
        public int Status { get; set; } = 1;

        [FieldMappingForMainTable]
        public long UserId { get; set; }
    }
}
