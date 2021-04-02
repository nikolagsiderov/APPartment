using System.Reflection;

namespace APPartment.UI.Utilities
{
    public class PropertyUIInfo
    {
        public PropertyUIInfo(PropertyInfo property)
        {
            Property = property;
        }

        public PropertyInfo Property { get; set; }

        public string Template { get; set; }

        public string DisplayName { get; set; }

        public int Row { get; set; } = 0;

        public int Col { get; set; } = 0;

        public int Order { get; set; } = 99;

        public string Section { get; set; }

        public string SelectList { get; set; }
    }
}
