using APPartment.Data.Models.Base;
using APPartment.Data.Models.Core;
using System.Collections.Generic;

namespace APPartment.UI.ViewModels.Home
{
    public class HomePageDisplayModel
    {
        public List<string> Messages { get; set; }

        public List<BaseObject> BaseObjects { get; set; }

        public HomeStatus HomeStatus { get; set; }

        public string RentDueDate { get; set; }
    }
}
