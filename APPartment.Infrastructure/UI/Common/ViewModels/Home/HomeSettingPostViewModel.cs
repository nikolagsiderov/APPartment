using APPartment.Data.Server.Models.Home;
using APPartment.Infrastructure.Services.Attributes;
using APPartment.Infrastructure.UI.Common.ViewModels.Base;
using APPartment.Infrastructure.UI.Web.Attributes;
using APPartment.Infrastructure.UI.Web.Constants;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace APPartment.Infrastructure.UI.Common.ViewModels.Home
{
    [IMapFrom(typeof(HomeSetting))]
    public class HomeSettingPostViewModel : PostViewModel
    {
        public override List<string> Sections => new List<string>() { "General" };

        [HiddenInput]
        [APPUIHint(Templates.Hidden)]
        public override string Name { get; set; }

        [HiddenInput]
        [APPUIHint(Templates.Hidden)]
        public override string Details { get; set; }

        [APPUIHint(Templates.Input, Row = 1, Col = 3, Order = 1, Section = "General")]
        [Display(Name = "Home name")]
        public string HomeName { get; set; }

        [APPUIHint(Templates.Number, Row = 2, Col = 2, Section = "General")]
        [Display(Name = "Rent due date day")]
        public int RentDueDateDay { get; set; }

        public bool ChangeHttpSession { get; set; }
    }
}
