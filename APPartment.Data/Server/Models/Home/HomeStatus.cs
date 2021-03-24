using APPartment.Data.Server.Models.Base;
using APPartment.ORM.Framework.Attributes;
using System.ComponentModel.DataAnnotations.Schema;

namespace APPartment.Data.Server.Models.Home
{
    [Table("HomeStatus", Schema = "dbo")]
    public class HomeStatus : HomeBaseObject
    {
        [FieldMappingForMainTable]
        public int Status { get; set; }

        [FieldMappingForMainTable]
        public long UserID { get; set; }
    }
}
