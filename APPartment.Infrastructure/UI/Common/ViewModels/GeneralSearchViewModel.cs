﻿using System.Collections.Generic;

namespace APPartment.Infrastructure.UI.Common.ViewModels
{
    public class GeneralSearchViewModel
    {
        public GeneralSearchViewModel()
        {

        }

        public GeneralSearchViewModel(List<BusinessObjectDisplayViewModel> result, string keyWords)
        {
            this.Result = result;
            this.KeyWords = keyWords;
        }

        public List<BusinessObjectDisplayViewModel> Result { get; set; } = new List<BusinessObjectDisplayViewModel>();

        public string KeyWords { get; set; }

        public double ElapsedTime { get; set; }
    }
}
