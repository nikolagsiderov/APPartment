using APPHome = APPartment.Data.Server.Models.Home.Home;
using System.ComponentModel.DataAnnotations;
using APPartment.Infrastructure.UI.Common.ViewModels.Base;
using APPartment.Infrastructure.Attributes;

namespace APPartment.Infrastructure.UI.Common.ViewModels.Home
{
    [IMapFrom(typeof(APPHome))]
    public class HomeDisplayViewModel : GridItemViewModel
    {
        [GridFieldDisplay(Order = 2)]
        [Display(Name = "Home name")]
        public override string Name { get; set; }

        public override string Details { get; set; }
    }
}
