using APPartment.Data.Server.Models.Base;

namespace APPartment.Data.Server.Models.Objects
{
    public class Chore : BaseObject
    {
        public long? AssignedToId { get; set; }
    }
}
