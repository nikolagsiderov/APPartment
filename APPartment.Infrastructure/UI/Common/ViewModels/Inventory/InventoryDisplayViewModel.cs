using APPartment.Infrastructure.Attributes;
using APPartment.Infrastructure.UI.Common.ViewModels.Base;
using System.ComponentModel.DataAnnotations;
using APPInventory = APPartment.Data.Server.Models.Inventory.Inventory;

namespace APPartment.Infrastructure.UI.Common.ViewModels.Inventory
{
    [IMapFrom(typeof(APPInventory))]
    public class InventoryDisplayViewModel : GridItemViewModel
    {
        [GridFieldDisplay]
        [Display(Name = "Supplied")]
        public bool IsSupplied { get; set; }
    }
}
