using APPartment.ORM.Framework.Attributes;
using APPartment.ORM.Framework.Declarations;

namespace APPartment.Data.Server.Models.Base
{
    public abstract class LookupObject : ILookupObject
    {
        [FieldMappingForLookupTable]
        public long ID { get; set; }

        [FieldMappingForLookupTable]
        public string Name { get; set; }

        [FieldMappingForLookupTable]
        public string Details { get; set; }
    }
}
