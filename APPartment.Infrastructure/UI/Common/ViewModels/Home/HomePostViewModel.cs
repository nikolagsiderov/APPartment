using APPHome = APPartment.Data.Server.Models.Home.Home;
using System.ComponentModel.DataAnnotations;
using APPartment.Infrastructure.UI.Common.ViewModels.Base;
using APPartment.Infrastructure.UI.Web.Constants;
using APPartment.Infrastructure.Attributes;
using System.Collections.Generic;

namespace APPartment.Infrastructure.UI.Common.ViewModels.Home
{
    [IMapFrom(typeof(APPHome))]
    public class HomePostViewModel : PostViewModel
    {
        public override List<string> Sections => new List<string>() { "General Information", "Settings" };

        [Required]
        [Display(Name = "Home name")]
        [APPUIHint(Templates.Input, Row = 1, Col = 6, Order = 1, Section = "General Information")]
        public override string Name { get; set; }

        [APPUIHint(Templates.Hidden)]
        public override string Details { get; set; }

        [Required(ErrorMessage = "Home password is required.")]
        [DataType(DataType.Password)]
        [APPUIHint(Templates.Password, Row = 2, Col = 6, Order = 1, Section = "General Information")]
        public string Password { get; set; }

        [APPUIHint(Templates.NumberNullable, Row = 1, Col = 3, Order = 1, Section = "Settings")]
        [Display(Name = "Rent due day")]
        public int? RentDueDay { get; set; }
    }
}
