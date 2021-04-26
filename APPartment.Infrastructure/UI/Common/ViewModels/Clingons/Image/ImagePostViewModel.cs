using APPImage = APPartment.Data.Server.Models.Clingons.Image;
using APPartment.Infrastructure.UI.Common.ViewModels.Base;
using APPartment.Infrastructure.UI.Web.Constants;
using APPartment.Infrastructure.Attributes;

namespace APPartment.Infrastructure.UI.Common.ViewModels.Clingons.Image
{
    [IMapFrom(typeof(APPImage))]
    public class ImagePostViewModel : PostViewModel
    {
        [APPUIHint(Templates.Input)]
        public string FileSize { get; set; }

        [APPUIHint(Templates.Hidden)]
        public long TargetObjectID { get; set; }
    }
}
