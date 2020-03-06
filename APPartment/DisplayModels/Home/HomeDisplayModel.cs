using APPartment.Models;
using APPartment.Models.Base;
using System.Collections.Generic;

namespace APPartment.DisplayModels.Home
{
    public class HomeDisplayModel
    {
        public List<string> Messages { get; set; }

        public List<BaseObject> BaseObjects { get; set; }

        public HouseStatus HouseStatus { get; set; }

        public string RentDueDate { get; set; }
    }
}
