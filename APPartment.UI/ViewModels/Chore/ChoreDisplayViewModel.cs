using APPartment.UI.Attributes;
using APPartment.UI.ViewModels.Base;
using System.ComponentModel.DataAnnotations;
using APPChore = APPartment.Data.Server.Models.Chore.Chore;

namespace APPartment.UI.ViewModels.Chore
{
    [IMapFrom(typeof(APPChore))]
    public class ChoreDisplayViewModel : GridItemViewModelWithHome
    {
        [Display(Name = "Assigned to")]
        public long? AssignedToUserID { get; set; }

        [GridFieldDisplay]
        [Display(Name = "Done")]
        public bool IsDone { get; set; }
    }
}
