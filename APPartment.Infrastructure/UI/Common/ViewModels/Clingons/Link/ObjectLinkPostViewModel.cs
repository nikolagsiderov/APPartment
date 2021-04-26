using APPartment.Infrastructure.UI.Common.ViewModels.Base;
using APPObjectLink = APPartment.Data.Server.Models.Clingons.ObjectLink;
using APPartment.Infrastructure.UI.Web.Constants;
using APPartment.Infrastructure.Attributes;

namespace APPartment.Infrastructure.UI.Common.ViewModels.Clingons.Link
{
    [IMapFrom(typeof(APPObjectLink))]
    public class ObjectLinkPostViewModel : PostViewModel
    {
        [APPUIHint(Templates.Dropdown, SelectList = "ObjectBIDSelectList")]
        public long ObjectBID { get; set; }

        [APPUIHint(Templates.Dropdown, SelectList = "ObjectLinkTypeSelectList")]
        public string ObjectLinkType { get; set; }

        [APPUIHint(Templates.Hidden)]
        public long TargetObjectID { get; set; }

        public string ObjectBName { get; set; }
    }
}
