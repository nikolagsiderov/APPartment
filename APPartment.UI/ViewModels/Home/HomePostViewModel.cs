using APPartment.UI.Attributes;
using APPartment.UI.Utilities.Constants;
using APPartment.UI.ViewModels.Base;
using APPHome = APPartment.Data.Server.Models.Home.Home;
using System.ComponentModel.DataAnnotations;

namespace APPartment.UI.ViewModels.Home
{
    [IMapFrom(typeof(APPHome))]
    public class HomePostViewModel : PostViewModel
    {
        [Required(ErrorMessage = "Home password is required.")]
        [DataType(DataType.Password)]
        [APPUIHint(Templates.Password)]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Please confirm your password.")]
        [DataType(DataType.Password)]
        [APPUIHint(Templates.Password)]
        public string ConfirmPassword { get; set; }
    }
}
