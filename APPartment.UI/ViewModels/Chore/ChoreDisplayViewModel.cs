using APPartment.UI.Attributes;
using APPartment.UI.ViewModels.Base;
using APPChore = APPartment.Data.Server.Models.Chore.Chore;

namespace APPartment.UI.ViewModels.Chore
{
    [IMapFrom(typeof(APPChore))]
    public class ChoreDisplayViewModel : GridItemViewModelWithHome
    {
        public long? AssignedToUserId { get; set; }

        public bool IsDone { get; set; }
    }
}
