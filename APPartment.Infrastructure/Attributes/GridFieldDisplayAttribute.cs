using System;

namespace APPartment.Infrastructure.Attributes
{
    public class GridFieldDisplayAttribute : Attribute
    {
        public int Order { get; set; } = 99;
    }
}
