using APPartment.UI.Attributes;
using APPartment.UI.Utilities.Constants;
using APPartment.UI.ViewModels.Base;
using System.ComponentModel.DataAnnotations;
using APPHygiene = APPartment.Data.Server.Models.Hygiene.Hygiene;

namespace APPartment.UI.ViewModels.Hygiene
{
    [IMapFrom(typeof(APPHygiene))]
    public class HygienePostViewModel : PostViewModelWithHome
    {
        [APPUIHint(Templates.Boolean)]
        [Display(Name = "Done")]
        public bool IsDone { get; set; }
    }
}
