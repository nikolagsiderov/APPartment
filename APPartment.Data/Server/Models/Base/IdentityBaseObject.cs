using APPartment.ORM.Framework.Attributes;
using APPartment.ORM.Framework.Declarations;

namespace APPartment.Data.Server.Models.Base
{
    public abstract class IdentityBaseObject : BaseObject, IIdentityBaseObject
    {
        [FieldMappingForMainTablePrimaryKey]
        public long Id { get; set; }
    }
}
