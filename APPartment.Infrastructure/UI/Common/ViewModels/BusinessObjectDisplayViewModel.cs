using APPartment.Data.Server.Models;
using APPartment.Infrastructure.Attributes;
using APPartment.Infrastructure.UI.Common.ViewModels.Base;

namespace APPartment.Infrastructure.UI.Common.ViewModels
{
    [IMapFrom(typeof(BusinessObject))]
    public class BusinessObjectDisplayViewModel : PostViewModel
    {
        public string ObjectTypeName { get; set; }

        public string Area { get; set; }

        public string WebLink { get; set; }
    }
}
