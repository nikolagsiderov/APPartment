using APPartment.UI.Attributes;
using APPartment.UI.Utilities.Constants;
using APPartment.UI.ViewModels.Base;
using APPImage = APPartment.Data.Server.Models.Clingons.Image;

namespace APPartment.UI.ViewModels.Clingons.Image
{
    [IMapFrom(typeof(APPImage))]
    public class ImagePostViewModel : PostViewModel
    {
        [APPUIHint(Templates.Input)]
        public string FileSize { get; set; }

        [APPUIHint(Templates.Hidden)]
        public long TargetObjectId { get; set; }
    }
}
