using APPartment.Models;
using APPartment.Models.Base;
using System.Collections.Generic;

namespace APPartment.DisplayModels.Home
{
    public class HomePageDisplayModel
    {
        public List<string> Messages { get; set; }

        public List<BaseObject> BaseObjects { get; set; }

        public HomeStatus HomeStatus { get; set; }

        public string RentDueDate { get; set; }
    }
}
