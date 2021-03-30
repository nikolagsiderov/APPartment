﻿using APPartment.UI.ViewModels.Base;
using System.Collections.Generic;

namespace APPartment.UI.ViewModels.Home
{
    public class HomePageDisplayModel
    {
        public List<string> Messages { get; set; }

        public HomeStatusPostViewModel HomeStatus { get; set; }

        public string RentDueDate { get; set; }
    }
}
