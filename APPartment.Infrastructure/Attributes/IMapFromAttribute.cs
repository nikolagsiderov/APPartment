using System;

namespace APPartment.Infrastructure.Attributes
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
