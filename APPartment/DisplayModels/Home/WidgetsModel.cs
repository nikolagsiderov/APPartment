using APPartment.Models.Base;
using System.Collections.Generic;

namespace APPartment.DisplayModels.Home
{
    public class WidgetsModel
    {
        public List<BaseObject> DisplayObjects { get; set; }

        public List<string> ObjectsLastUpdate { get; set; }
    }
}
