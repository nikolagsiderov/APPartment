using APPartment.UI.Attributes;
using APPartment.UI.Utilities.Constants;

namespace APPartment.UI.ViewModels.Base
{
    public abstract class PostViewModelWithHome : PostViewModel
    {
        [APPUIHint(Templates.Hidden)]
        public long HomeId { get; set; }
    }
}
