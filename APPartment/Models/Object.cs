using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APPartment.Models
{
    public class Object
    {
        [Key]
        public long ObjectId { get; set; }

        public long CreatedById { get; set; }

        public DateTime CreatedDate { get; set; }
        
        public long ModifiedById { get; set; }

        public DateTime ModifiedDate { get; set; }

        [ForeignKey("ObjectType")]
        public long ObjectTypeId { get; set; }
    }
}
