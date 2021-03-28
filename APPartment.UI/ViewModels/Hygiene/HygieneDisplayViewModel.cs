using APPartment.UI.Attributes;
using APPartment.UI.ViewModels.Base;
using System.ComponentModel.DataAnnotations;
using APPHygiene = APPartment.Data.Server.Models.Hygiene.Hygiene;

namespace APPartment.UI.ViewModels.Hygiene
{
    [IMapFrom(typeof(APPHygiene))]
    public class HygieneDisplayViewModel : GridItemViewModelWithHome
    {
        [GridFieldDisplay]
        [Display(Name = "Done")]
        public bool IsDone { get; set; }
    }
}
