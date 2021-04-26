using APPartment.Infrastructure.Attributes;
using APPartment.Infrastructure.UI.Web.Constants;
using APPartment.ORM.Framework.Declarations;

namespace APPartment.Infrastructure.UI.Common.ViewModels.Base
{
    public abstract class LookupPostViewModel : ILookupObject
    {
        [APPUIHint(Templates.Hidden)]
        public long ID { get; set; }

        [APPUIHint(Templates.Input)]
        public string Name { get; set; }

        [APPUIHint(Templates.TextArea)]
        public string Details { get; set; }
    }
}
