using APPartment.UI.Attributes;
using APPartment.UI.Utilities.Constants;
using APPartment.UI.ViewModels.Base;
using APPChore = APPartment.Data.Server.Models.Chore.Chore;

namespace APPartment.UI.ViewModels.Chore
{
    [IMapFrom(typeof(APPChore))]
    public class ChorePostViewModel : PostViewModelWithHome
    {
        [APPUIHint(Templates.Hidden)]
        public long? AssignedToUserId { get; set; }
    }
}
