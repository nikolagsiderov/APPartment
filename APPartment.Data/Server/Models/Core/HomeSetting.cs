using APPartment.Data.Server.Models.Base;
using APPartment.ORM.Framework.Attributes;
using System.ComponentModel.DataAnnotations.Schema;

namespace APPartment.Data.Server.Models.Core
{
    [Table("HomeSetting", Schema = "dbo")]
    public class HomeSetting : HomeBaseObject
    {
        [FieldMappingForMainTable]
        public int RentDueDateDay { get; set; }

        [FieldMappingForMainTable]
        public string HomeName { get; set; }
    }
}
