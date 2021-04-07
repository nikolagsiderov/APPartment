using System;

namespace APPartment.Infrastructure.UI.Web.Attributes
{
    public class GridFieldDisplayAttribute : Attribute
    {
        public int Order { get; set; } = 99;
    }
}
