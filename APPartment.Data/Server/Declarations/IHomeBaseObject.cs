using APPartment.Data.Attributes;

namespace APPartment.Data.Server.Declarations
{
    public interface IHomeBaseObject : IIdentityBaseObject
    {
        [FieldMappingForMainTable]
        public long HomeId { get; set; }
    }
}
