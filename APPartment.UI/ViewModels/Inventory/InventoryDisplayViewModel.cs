using APPartment.UI.Attributes;
using APPartment.UI.ViewModels.Base;
using APPInventory = APPartment.Data.Server.Models.Inventory.Inventory;

namespace APPartment.UI.ViewModels.Inventory
{
    [IMapFrom(typeof(APPInventory))]
    public class InventoryDisplayViewModel : GridItemViewModelWithHome
    {
        [GridFieldDisplay]
        public bool IsSupplied { get; set; }
    }
}
