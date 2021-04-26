using APPUser = APPartment.Data.Server.Models.User.User;
using APPartment.Infrastructure.UI.Common.ViewModels.Base;
using System.ComponentModel.DataAnnotations;
using APPartment.Infrastructure.Attributes;

namespace APPartment.Infrastructure.UI.Common.ViewModels.User
{
    [IMapFrom(typeof(APPUser))]
    public class UserDisplayViewModel : GridItemViewModel
    {
        [GridFieldDisplay(Order = 2)]
        [Display(Name = "Username")]
        public override string Name { get; set; }

        public override string Details { get; set; }
    }
}
