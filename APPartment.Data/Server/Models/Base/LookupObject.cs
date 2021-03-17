using APPartment.ORM.Framework.Attributes;
using APPartment.ORM.Framework.Declarations;
using APPartment.UI.Attributes;

namespace APPartment.Data.Server.Models.Base
{
    public abstract class LookupObject : ILookupObject
    {
        [FieldMappingForLookupTable]
        [APPUIHint(Templates.Hidden)]
        public long Id { get; set; }

        [FieldMappingForLookupTable]
        [APPUIHint(Templates.Input)]
        public string Name { get; set; }

        [FieldMappingForLookupTable]
        [APPUIHint(Templates.TextArea)]
        public string Details { get; set; }
    }
}
