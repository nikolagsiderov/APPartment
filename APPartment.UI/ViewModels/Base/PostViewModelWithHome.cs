using APPartment.UI.Attributes;
using APPartment.UI.Utilities.Constants;

namespace APPartment.UI.ViewModels.Base
{
    public abstract class PostViewModelWithHome : PostViewModel
    {
        #region Hidden properties
        [APPUIHint(Templates.Hidden)]
        public long HomeID { get; set; }
        #endregion
    }
}
