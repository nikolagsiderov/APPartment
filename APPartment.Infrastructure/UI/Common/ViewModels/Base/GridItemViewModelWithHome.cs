namespace APPartment.Infrastructure.UI.Common.ViewModels.Base
{
    public abstract class GridItemViewModelWithHome : GridItemViewModel
    {
        #region Hidden properties
        public long HomeID { get; set; }
        #endregion
    }
}