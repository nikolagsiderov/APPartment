using APPartment.Infrastructure.Services.Attributes;
using APPartment.Infrastructure.UI.Common.ViewModels.Base;
using APPartment.Infrastructure.UI.Web.Attributes;
using APPartment.Infrastructure.UI.Web.Constants;
using System.ComponentModel.DataAnnotations;
using APPHygiene = APPartment.Data.Server.Models.Hygiene.Hygiene;

namespace APPartment.Infrastructure.UI.Common.ViewModels.Hygiene
{
    [IMapFrom(typeof(APPHygiene))]
    public class HygienePostViewModel : PostViewModelWithHome
    {
        [APPUIHint(Templates.Boolean, Row = 1, Col = 6, Order = 2, Section = "General Information")]
        [Display(Name = "Done")]
        public bool IsDone { get; set; }
    }
}
