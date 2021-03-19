using System;

namespace APPartment.UI.Attributes
{
    public class IMapFromAttribute : Attribute
    {
        public IMapFromAttribute(Type serverModelType)
        {
            ServerModelType = serverModelType;
        }

        public Type ServerModelType { get; set; } 
    }
}
