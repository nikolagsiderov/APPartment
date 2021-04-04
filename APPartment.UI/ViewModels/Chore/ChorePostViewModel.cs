using APPartment.UI.Attributes;
using APPartment.UI.Constants;
using APPartment.UI.ViewModels.Base;
using System.ComponentModel.DataAnnotations;
using APPChore = APPartment.Data.Server.Models.Chore.Chore;

namespace APPartment.UI.ViewModels.Chore
{
    [IMapFrom(typeof(APPChore))]
    public class ChorePostViewModel : PostViewModelWithHome
    {
        [APPUIHint(Templates.Boolean, Row = 1, Col = 6, Order = 2, Section = "General Information")]
        [Display(Name = "Done")]
        public bool IsDone { get; set; }

        [APPUIHint(Templates.Dropdown, Row = 3, Col = 4, Order = 1, Section = "General Information", SelectList = "AssignedToUserID")]
        [Display(Name = "Assigned to")]
        public long? AssignedToUserID { get; set; }
    }
}
