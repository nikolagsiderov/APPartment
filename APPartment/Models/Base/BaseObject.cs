using APPartment.Models.Declaration;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APPartment.Models.Base
{
    public abstract class BaseObject : IBaseObject
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Details { get; set; }

        public int Status { get; set; }

        [ForeignKey("Home")]
        public long? HomeId { get; set; }

        [ForeignKey("Object")]
        public long ObjectId { get; set; }

        [NotMapped]
        public List<string> Comments { get; set; } = new List<string>();

        [NotMapped]
        public List<Image> Images { get; set; } = new List<Image>();

        [NotMapped]
        public List<string> History { get; set; } = new List<string>();

        [NotMapped]
        public string LastUpdated { get; set; }

        [NotMapped]
        public string LastUpdatedBy { get; set; }

        [NotMapped]
        public string LastUpdate { get; set; }
    }
}
