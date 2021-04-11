using APPartment.Infrastructure.Services.Attributes;
using APPartment.Infrastructure.UI.Common.ViewModels.Base;
using APPartment.Infrastructure.UI.Web.Attributes;
using APPartment.Infrastructure.UI.Web.Constants;
using System.ComponentModel.DataAnnotations;
using APPInventory = APPartment.Data.Server.Models.Inventory.Inventory;

namespace APPartment.Infrastructure.UI.Common.ViewModels.Inventory
{
    [IMapFrom(typeof(APPInventory))]
    public class InventoryPostViewModel : PostViewModel
    {
        [APPUIHint(Templates.Boolean, Row = 1, Col = 6, Order = 2, Section = "General Information")]
        [Display(Name = "Supplied")]
        public bool IsSupplied { get; set; }
    }
}
