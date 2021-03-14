using APPartment.ORM.Framework.Attributes;

namespace APPartment.ORM.Framework.Declarations
{
    public interface ILookupObject
    {
        [FieldMappingForLookupTable]
        public long Id { get; set; }

        [FieldMappingForLookupTable]
        public string Name { get; set; }

        [FieldMappingForLookupTable]
        public string Details { get; set; }
    }
}
