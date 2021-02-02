using APPartment.Data.Models.Base;

namespace APPartment.Data.Models.Objects
{
    public class Chore : BaseObject
    {
        public long? AssignedToId { get; set; }
    }
}
