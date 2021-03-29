using System;

namespace APPartment.UI.Attributes
{
    public class GridFieldDisplayAttribute : Attribute
    {
        public int Order { get; set; } = 99;
    }
}
