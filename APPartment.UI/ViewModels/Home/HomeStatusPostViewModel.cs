using APPartment.UI.Attributes;
using APPartment.UI.Utilities.Constants;
using APPartment.UI.ViewModels.Base;
using APPartment.Data.Server.Models.Home;

namespace APPartment.UI.ViewModels.Home
{
    [IMapFrom(typeof(HomeStatus))]
    public class HomeStatusPostViewModel : PostViewModelWithHome
    {
        [APPUIHint(Templates.Integer)]
        public int Status { get; set; }

        [APPUIHint(Templates.Hidden)]
        public long UserId { get; set; }
    }
}
