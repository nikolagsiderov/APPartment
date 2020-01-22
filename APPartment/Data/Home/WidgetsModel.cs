using APPartment.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APPartment.Data.Home
{
    public class WidgetsModel
    {
        public List<BaseObject> DisplayObjects { get; set; }

        public List<string> ObjectsLastUpdate { get; set; }
    }
}
