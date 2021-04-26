using APPartment.Infrastructure.Attributes;
using APPartment.Infrastructure.UI.Common.ViewModels.Base;
using System.ComponentModel.DataAnnotations;
using APPChore = APPartment.Data.Server.Models.Chore.Chore;

namespace APPartment.Infrastructure.UI.Common.ViewModels.Chore
{
    [IMapFrom(typeof(APPChore))]
    public class ChoreDisplayViewModel : GridItemViewModel
    {
        [Display(Name = "Assigned to")]
        public long? AssignedToUserID { get; set; }

        [GridFieldDisplay]
        [Display(Name = "Done")]
        public bool IsDone { get; set; }
    }
}
