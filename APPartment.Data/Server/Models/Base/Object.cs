using APPartment.Data.Server.Declarations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APPartment.Data.Server.Models.Base
{
    public abstract class Object : IObject
    {
        [ForeignKey("Object")]
        public long ObjectId { get; set; }
    }
}
