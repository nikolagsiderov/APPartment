using APPartment.Models.Base;

namespace APPartment.Models
{
    public class Chore : BaseObject
    {
        public long? AssignedToId { get; set; }
    }
}
