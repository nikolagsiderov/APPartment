using DOTNETAreaAttribute = Microsoft.AspNetCore.Mvc.AreaAttribute;

namespace APPartment.Infrastructure.Attributes
{
    public class AreaAttribute : DOTNETAreaAttribute
    {
        public AreaAttribute(string areaName) : base(areaName)
        {
            AreaName = areaName;
        }

        public string AreaName { get; set; }
    }
}
