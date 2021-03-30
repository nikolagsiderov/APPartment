using APPartment.UI.Attributes;
using APPartment.UI.Utilities.Constants;
using APPartment.UI.ViewModels.Base;
using APPartment.Data.Server.Models.Home;
using System.ComponentModel.DataAnnotations;

namespace APPartment.UI.ViewModels.Home
{
    [IMapFrom(typeof(HomeSetting))]
    public class HomeSettingPostViewModel : PostViewModelWithHome
    {
        [APPUIHint(Templates.Integer)]
        [Display(Name = "Rent due date day")]
        public int RentDueDateDay { get; set; }

        [APPUIHint(Templates.Input)]
        [Display(Name = "Home name")]
        public string HomeName { get; set; }

        public bool ChangeHttpSession { get; set; }
    }
}
