using APPartment.ORM.Framework.Attributes;

namespace APPartment.ORM.Framework.Declarations
{
    public interface IIdentityBaseObject : IBaseObject
    {
        [FieldMappingForMainTablePrimaryKey]
        public long Id { get; set; }
    }
}
