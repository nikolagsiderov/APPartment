using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APPartment.Data.Server.Models.MetaObjects
{
    public class Link : Base.Object
    {
        [Key]
        public long Id { get; set; }

        [NotMapped]
        public string Display { get; set; }

        public long LinkTypeId { get; set; }

        public long LinkedObjectId { get; set; }

        public long CreatedById { get; set; }

        public DateTime CreatedDate { get; set; }

        public long TargetObjectId { get; set; }
    }
}
