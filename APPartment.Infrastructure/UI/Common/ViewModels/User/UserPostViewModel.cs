using APPUser = APPartment.Data.Server.Models.User.User;
using System.ComponentModel.DataAnnotations;
using APPartment.Infrastructure.Services.Attributes;
using APPartment.Infrastructure.UI.Common.ViewModels.Base;
using APPartment.Infrastructure.UI.Web.Attributes;
using APPartment.Infrastructure.UI.Web.Constants;

namespace APPartment.Infrastructure.UI.Common.ViewModels.User
{
    [IMapFrom(typeof(APPUser))]
    public class UserPostViewModel : PostViewModel
    {
        [Required]
        [Display(Name = "Username")]
        [APPUIHint(Templates.Input)]
        public override string Name { get; set; }
  
        [Required(ErrorMessage = "Password is required.")]
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
