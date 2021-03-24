using APPartment.UI.Attributes;
using APPartment.UI.ViewModels.Base;
using APPHygiene = APPartment.Data.Server.Models.Hygiene.Hygiene;

namespace APPartment.UI.ViewModels.Hygiene
{
    [IMapFrom(typeof(APPHygiene))]
    public class HygieneDisplayViewModel : GridItemViewModelWithHome
    {
        [GridFieldDisplay]
        public bool IsDone { get; set; }
    }
}
