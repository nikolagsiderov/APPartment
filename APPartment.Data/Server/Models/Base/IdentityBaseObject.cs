using APPartment.ORM.Framework.Attributes;
using APPartment.ORM.Framework.Declarations;
using APPartment.UI.Attributes;

namespace APPartment.Data.Server.Models.Base
{
    public abstract class IdentityBaseObject : BaseObject, IIdentityBaseObject
    {
        [FieldMappingForMainTablePrimaryKey]
        [APPUIHint(Templates.Hidden)]
        public long Id { get; set; }
    }
}
