using APPartment.UI.Attributes;
using APPartment.UI.Constants;
using APPartment.UI.ViewModels.Base;
using System.ComponentModel.DataAnnotations;
using APPInventory = APPartment.Data.Server.Models.Inventory.Inventory;

namespace APPartment.UI.ViewModels.Inventory
{
    [IMapFrom(typeof(APPInventory))]
    public class InventoryPostViewModel : PostViewModelWithHome
    {
        [APPUIHint(Templates.Boolean, Row = 1, Col = 6, Order = 2, Section = "General Information")]
        [Display(Name = "Supplied")]
        public bool IsSupplied { get; set; }
    }
}
