using APPartment.Infrastructure.UI.Web.Attributes;
using APPartment.Infrastructure.UI.Web.Constants;

namespace APPartment.Infrastructure.UI.Common.ViewModels.Base
{
    public abstract class PostViewModelWithHome : PostViewModel
    {
        #region Hidden properties
        [APPUIHint(Templates.Hidden)]
        public long HomeID { get; set; }
        #endregion
    }
}
