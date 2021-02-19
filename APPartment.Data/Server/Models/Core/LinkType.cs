using APPartment.Data.Attributes;
using APPartment.Data.Server.Models.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace APPartment.Data.Server.Models.Core
{
    [Table("LinkType", Schema = "dbo")]
    public class LinkType : HomeBaseObject
    {
        [FieldMappingForMainTable]
        public long ConnectedLinkTypeId { get; set; }
    }
}
