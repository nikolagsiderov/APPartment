using APPartment.UI.Attributes;
using APPartment.UI.Utilities.Constants;
using APPartment.UI.ViewModels.Base;
using APPartment.Data.Server.Models.Home;

namespace APPartment.UI.ViewModels.Home
{
    [IMapFrom(typeof(HomeSetting))]
    public class HomeSettingPostViewModel : PostViewModelWithHome
    {
        [APPUIHint(Templates.Integer)]
        public int RentDueDateDay { get; set; }

        [APPUIHint(Templates.Input)]
        public string HomeName { get; set; }
    }
}
