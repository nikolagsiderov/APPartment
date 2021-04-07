using APPHome = APPartment.Data.Server.Models.Home.Home;
using System.ComponentModel.DataAnnotations;
using APPartment.Infrastructure.Services.Attributes;
using APPartment.Infrastructure.UI.Common.ViewModels.Base;
using APPartment.Infrastructure.UI.Web.Constants;
using APPartment.Infrastructure.UI.Web.Attributes;

namespace APPartment.Infrastructure.UI.Common.ViewModels.Home
{
    [IMapFrom(typeof(APPHome))]
    public class HomePostViewModel : PostViewModel
    {
        [Display(Name = "Home name")]
        public override string Name { get; set; }

        [Required(ErrorMessage = "Home password is required.")]
        [DataType(DataType.Password)]
        [APPUIHint(Templates.Password)]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Please confirm your password.")]
        [DataType(DataType.Password)]
        [APPUIHint(Templates.Password)]
        [Display(Name = "Confirm password")]
        public string ConfirmPassword { get; set; }
    }
}
