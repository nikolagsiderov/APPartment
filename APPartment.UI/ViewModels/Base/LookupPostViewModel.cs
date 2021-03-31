﻿using APPartment.ORM.Framework.Declarations;
using APPartment.UI.Attributes;
using APPartment.UI.Constants;

namespace APPartment.UI.ViewModels.Base
{
    public abstract class LookupPostViewModel : ILookupObject
    {
        [APPUIHint(Templates.Hidden)]
        public long ID { get; set; }

        [APPUIHint(Templates.Input)]
        public string Name { get; set; }

        [APPUIHint(Templates.TextArea)]
        public string Details { get; set; }
    }
}
