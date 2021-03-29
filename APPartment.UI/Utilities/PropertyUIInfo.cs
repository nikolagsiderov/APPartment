using System.Reflection;

namespace APPartment.UI.Utilities
{
    public class PropertyUIInfo
    {
        public PropertyInfo Property { get; set; }

        public string DisplayName { get; set; }

        public int Row { get; set; } = 0;

        public int Col { get; set; } = 0;

        public int ColDiv { get; set; } = 0;
    }
}
