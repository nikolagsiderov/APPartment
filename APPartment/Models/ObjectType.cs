using System.ComponentModel.DataAnnotations;

namespace APPartment.Models
{
    public class ObjectType
    {
        [Key]
        public long Id { get; set; }

        public string Name { get; set; }
    }
}
