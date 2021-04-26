using APPartment.Infrastructure.Attributes;
using APPartment.Infrastructure.UI.Common.ViewModels.Base;
using APPartment.Infrastructure.UI.Web.Constants;
using System.ComponentModel.DataAnnotations;
using APPChore = APPartment.Data.Server.Models.Chore.Chore;

namespace APPartment.Infrastructure.UI.Common.ViewModels.Chore
{
    [IMapFrom(typeof(APPChore))]
    public class ChorePostViewModel : PostViewModel
    {
        [APPUIHint(Templates.Boolean, Row = 1, Col = 6, Order = 2, Section = "General Information")]
        [Display(Name = "Done")]
        public bool IsDone { get; set; }

        [APPUIHint(Templates.Dropdown, Row = 3, Col = 4, Order = 1, Section = "General Information", SelectList = "AssignedToUserID")]
        [Display(Name = "Assigned to")]
        public long? AssignedToUserID { get; set; }
    }
}
