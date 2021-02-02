using APPartment.Data.Models.MetaObjects;
using System.Collections.Generic;

namespace APPartment.Data.Models.Declarations
{
    public interface IBaseObject : IObject
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Details { get; set; }

        public int Status { get; set; }

        public long? HomeId { get; set; }

        public List<string> Comments { get; set; }

        public List<Image> Images { get; set; }

        public List<string> History { get; set; }
    }
}
