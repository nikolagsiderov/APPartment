using APPartment.ORM.Framework.Enums;
using System;

namespace APPartment.ORM.Framework.Helpers
{
    public static class ObjectTypeDeterminator
    {
        public static long GetObjectTypeID(string objectTypeName)
        {
            var objectType = (ObjectTypes)Enum.Parse(typeof(ObjectTypes), objectTypeName);
            return (long)objectType;
        }
    }
}
