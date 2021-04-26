using APPUser = APPartment.Data.Server.Models.User.User;
using System.ComponentModel.DataAnnotations;
using APPartment.Infrastructure.UI.Common.ViewModels.Base;
using APPartment.Infrastructure.UI.Web.Constants;
using APPartment.Infrastructure.Attributes;

namespace APPartment.Infrastructure.UI.Common.ViewModels.User
{
    [IMapFrom(typeof(APPUser))]
    public class UserPostViewModel : PostViewModel
    {
        [Required]
        [Display(Name = "Username")]
        [APPUIHint(Templates.Input, Row = 1, Col = 6, Order = 1, Section = "General Information")]
        public override string Name { get; set; }

        [APPUIHint(Templates.Hidden)]
        public override string Details { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        [APPUIHint(Templates.Password, Row = 2, Col = 6, Order = 1, Section = "General Information")]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Please confirm your password.")]
        [DataType(DataType.Password)]
        [APPUIHint(Templates.Password)]
        [Display(Name = "Confirm password")]
        public string ConfirmPassword { get; set; }
    }
}
