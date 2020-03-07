using APPartment.Models.Declaration;
using System.ComponentModel.DataAnnotations.Schema;

namespace APPartment.Models.Base
{
    public abstract class Object : IObject
    {
        [ForeignKey("Object")]
        public long ObjectId { get; set; }

        [ForeignKey("ObjectType")]
        public long ObjectTypeId { get; set; }
    }
}
