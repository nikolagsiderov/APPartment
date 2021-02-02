using APPartment.Data.Models.Declarations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APPartment.Data.Models.Base
{
    public abstract class Object : IObject
    {
        [ForeignKey("Object")]
        public long ObjectId { get; set; }
    }
}
