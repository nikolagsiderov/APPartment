using APPartment.Models.Declaration;
using System;
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

        public bool IsCompleted { get; set; } = false;

        public string CreatedBy { get; set; }

        public string ModifiedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [ForeignKey("House")]
        public long? HouseId { get; set; }

        [ForeignKey("Object")]
        public long ObjectId { get; set; }

        [NotMapped]
        public List<string> Comments { get; set; }

        [NotMapped]
        public List<Image> Images { get; set; }
    }
}
