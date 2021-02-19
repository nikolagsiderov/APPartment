using APPartment.Data.Attributes;
using APPartment.Data.Server.Declarations;

namespace APPartment.Data.Server.Models.Base
{
    public abstract class IdentityBaseObject : BaseObject, IIdentityBaseObject
    {
        [FieldMappingForMainTablePrimaryKey]
        public long Id { get; set; }
    }
}
