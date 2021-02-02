using APPartment.Data.Models.Base;

namespace APPartment.Data.Models.Objects
{
    public class Survey : BaseObject
    {
        public bool IsCompleted { get; set; } = false;
    }
}
