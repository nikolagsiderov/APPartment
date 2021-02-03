using APPartment.Data.Server.Models.Base;

namespace APPartment.Data.Server.Models.Objects
{
    public class Survey : BaseObject
    {
        public bool IsCompleted { get; set; } = false;
    }
}
