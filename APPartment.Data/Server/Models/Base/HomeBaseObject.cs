using APPartment.ORM.Framework.Attributes;
using APPartment.ORM.Framework.Declarations;
using APPartment.UI.Attributes;

namespace APPartment.Data.Server.Models.Base
{
    public class HomeBaseObject : IdentityBaseObject, IHomeBaseObject
    {
        [FieldMappingForMainTable]
        [APPUIHint(Templates.Hidden)]
        public long HomeId { get; set; }
    }
}
