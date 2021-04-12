using APPartment.Infrastructure.Services.Attributes;
using APPHome = APPartment.Data.Server.Models.Home.Home;
using APPartment.Infrastructure.UI.Web.Attributes;
using System.ComponentModel.DataAnnotations;
using APPartment.Infrastructure.UI.Common.ViewModels.Base;

namespace APPartment.Infrastructure.UI.Common.ViewModels.Home
{
    [IMapFrom(typeof(APPHome))]
    public class HomeDisplayViewModel : GridItemViewModel
    {
        [GridFieldDisplay(Order = 2)]
        [Display(Name = "Home name")]
        public override string Name { get; set; }
    }
}
