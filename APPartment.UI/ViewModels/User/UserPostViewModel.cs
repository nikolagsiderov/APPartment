using APPartment.UI.Attributes;
using APPartment.UI.Utilities.Constants;
using APPartment.UI.ViewModels.Base;
using APPUser = APPartment.Data.Server.Models.User.User;
using System.ComponentModel.DataAnnotations;

namespace APPartment.UI.ViewModels.User
{
    [IMapFrom(typeof(APPUser))]
    public class UserPostViewModel : PostViewModel
    {
        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        [APPUIHint(Templates.Password)]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Please confirm your password.")]
        [DataType(DataType.Password)]
        [APPUIHint(Templates.Password)]
        public string ConfirmPassword { get; set; }
    }
}
