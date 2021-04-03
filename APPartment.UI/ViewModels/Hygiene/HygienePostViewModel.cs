using APPartment.UI.Attributes;
using APPartment.UI.Constants;
using APPartment.UI.ViewModels.Base;
using System.ComponentModel.DataAnnotations;
using APPHygiene = APPartment.Data.Server.Models.Hygiene.Hygiene;

namespace APPartment.UI.ViewModels.Hygiene
{
    [IMapFrom(typeof(APPHygiene))]
    public class HygienePostViewModel : PostViewModelWithHome
    {
        [APPUIHint(Templates.Boolean, Row = 1, Col = 6, Order = 2, Section = "General Information")]
        [Display(Name = "Done")]
        public bool IsDone { get; set; }
    }
}
