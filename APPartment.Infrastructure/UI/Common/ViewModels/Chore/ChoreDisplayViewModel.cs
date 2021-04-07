using APPartment.Infrastructure.Services.Attributes;
using APPartment.Infrastructure.UI.Common.ViewModels.Base;
using APPartment.Infrastructure.UI.Web.Attributes;
using System.ComponentModel.DataAnnotations;
using APPChore = APPartment.Data.Server.Models.Chore.Chore;

namespace APPartment.Infrastructure.UI.Common.ViewModels.Chore
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
