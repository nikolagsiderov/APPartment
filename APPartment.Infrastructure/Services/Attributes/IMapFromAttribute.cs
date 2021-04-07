using System;

namespace APPartment.Infrastructure.Services.Attributes
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
