using APPartment.Data.Server.Models.Home;
using APPartment.Infrastructure.Services.Attributes;
using APPartment.Infrastructure.UI.Common.ViewModels.Base;
using APPartment.Infrastructure.UI.Web.Attributes;
using APPartment.Infrastructure.UI.Web.Constants;
using System.ComponentModel.DataAnnotations;

namespace APPartment.Infrastructure.UI.Common.ViewModels.Home
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
