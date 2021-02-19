using APPartment.Data.Attributes;
using APPartment.Data.Server.Declarations;

namespace APPartment.Data.Server.Models.Base
{
    public class HomeBaseObject : IdentityBaseObject, IHomeBaseObject
    {
        [FieldMappingForMainTable]
        public long HomeId { get; set; }
    }
}
