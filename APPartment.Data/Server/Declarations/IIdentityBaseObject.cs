using APPartment.Data.Attributes;

namespace APPartment.Data.Server.Declarations
{
    public interface IIdentityBaseObject : IBaseObject
    {
        [FieldMappingForMainTablePrimaryKey]
        public long Id { get; set; }
    }
}
