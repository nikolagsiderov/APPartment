using APPartment.ORM.Framework.Attributes;
using APPartment.ORM.Framework.Declarations;

namespace APPartment.Data.Server.Models.Base
{
    public class HomeBaseObject : IdentityBaseObject, IHomeBaseObject
    {
        [FieldMappingForMainTable]
        public long HomeId { get; set; }
    }
}
