using APPartment.ORM.Framework.Attributes;

namespace APPartment.ORM.Framework.Declarations
{
    public interface IHomeBaseObject : IIdentityBaseObject
    {
        [FieldMappingForMainTable]
        public long HomeId { get; set; }
    }
}
