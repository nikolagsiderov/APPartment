using System.Collections.Generic;

namespace APPartment.Infrastructure.UI.Common.ViewModels.Home
{
    public class HomePageDisplayModel
    {
        public List<string> Messages { get; set; }

        public string InventoryLastUpdate { get; set; }

        public string ChoresLastUpdate { get; set; }

        public string IssuesLastUpdate { get; set; }

        public string RentDueDisplay { get; set; }
    }
}
