using APPartment.Data.Server.Models.Base;
using APPartment.ORM.Framework.Attributes;
using System.ComponentModel.DataAnnotations.Schema;

namespace APPartment.Data.Server.Models.Home
{
    [Table("HomeSetting", Schema = "dbo")]
    public class HomeSetting : BaseObject
    {
        [FieldMappingForMainTable]
        public int RentDueDateDay { get; set; }

        [FieldMappingForMainTable]
        public string HomeName { get; set; }
    }
}
