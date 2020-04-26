using System.Collections.Generic;

namespace APPartment.Models.Declaration
{
    public interface IBaseObject : IObject
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Details { get; set; }

        public int Status { get; set; }

        public bool Marked { get; set; }

        public long? HouseId { get; set; }

        public List<string> Comments { get; set; }

        public List<Image> Images { get; set; }

        public List<string> History { get; set; }
    }
}
