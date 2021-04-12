using APPartment.Infrastructure.Services.Attributes;
using APPUser = APPartment.Data.Server.Models.User.User;
using APPartment.Infrastructure.UI.Common.ViewModels.Base;
using APPartment.Infrastructure.UI.Web.Attributes;
using System.ComponentModel.DataAnnotations;

namespace APPartment.Infrastructure.UI.Common.ViewModels.User
{
    [IMapFrom(typeof(APPUser))]
    public class UserDisplayViewModel : GridItemViewModel
    {
        [GridFieldDisplay(Order = 2)]
        [Display(Name = "Username")]
        public override string Name { get; set; }
    }
}
