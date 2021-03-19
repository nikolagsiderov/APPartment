using APPartment.UI.Attributes;
using APPartment.UI.Utilities.Constants;
using APPartment.UI.ViewModels.Base;
using APPartment.Data.Server.Models.Home;

namespace APPartment.UI.ViewModels.Home
{
    [IMapFrom(typeof(HomeUser))]
    public class HomeUserPostViewModel : PostViewModelWithHome
    {
        [APPUIHint(Templates.Hidden)]
        public long UserId { get; set; }
    }
}
