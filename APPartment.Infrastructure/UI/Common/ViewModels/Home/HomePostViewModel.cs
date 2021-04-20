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
        [Required]
        [Display(Name = "Home name")]
        [APPUIHint(Templates.Input, Row = 1, Col = 6, Order = 1, Section = "General Information")]
        public override string Name { get; set; }

        [APPUIHint(Templates.Hidden)]
        public override string Details { get; set; }

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
