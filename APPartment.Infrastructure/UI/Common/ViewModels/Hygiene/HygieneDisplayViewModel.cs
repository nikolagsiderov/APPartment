using APPartment.Infrastructure.Services.Attributes;
using APPartment.Infrastructure.UI.Common.ViewModels.Base;
using APPartment.Infrastructure.UI.Web.Attributes;
using System.ComponentModel.DataAnnotations;
using APPHygiene = APPartment.Data.Server.Models.Hygiene.Hygiene;

namespace APPartment.Infrastructure.UI.Common.ViewModels.Hygiene
{
    [IMapFrom(typeof(APPHygiene))]
    public class HygieneDisplayViewModel : GridItemViewModelWithHome
    {
        [GridFieldDisplay]
        [Display(Name = "Done")]
        public bool IsDone { get; set; }
    }
}
