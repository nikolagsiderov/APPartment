using APPartment.Data.Server.Models.Home;
using APPartment.Infrastructure.Services.Attributes;
using APPartment.Infrastructure.UI.Common.ViewModels.Base;
using APPartment.Infrastructure.UI.Web.Attributes;
using APPartment.Infrastructure.UI.Web.Constants;

namespace APPartment.Infrastructure.UI.Common.ViewModels.Home
{
    [IMapFrom(typeof(HomeUser))]
    public class HomeUserPostViewModel : PostViewModel
    {
        [APPUIHint(Templates.Hidden)]
        public long UserID { get; set; }
    }
}
