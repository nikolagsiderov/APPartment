using System.ComponentModel.DataAnnotations;

namespace APPartment.Data.Models.Core
{
    public class ObjectType
    {
        [Key]
        public long Id { get; set; }

        public string Name { get; set; }
    }
}
