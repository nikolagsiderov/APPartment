using APPartment.UI.Attributes;
using APPartment.UI.Utilities.Constants;
using APPartment.UI.ViewModels.Base;
using APPInventory = APPartment.Data.Server.Models.Inventory.Inventory;

namespace APPartment.UI.ViewModels.Inventory
{
    [IMapFrom(typeof(APPInventory))]
    public class InventoryPostViewModel : PostViewModelWithHome
    {
    }
}
