using System;
using System.ComponentModel.DataAnnotations;

namespace APPartment.Data.Models.MetaObjects
{
    public class Image : Base.Object
    {
        [Key]
        public long Id { get; set; }

        public string Name { get; set; }

        public string FileName { get; set; }

        public string FileSize { get; set; }

        public DateTime CreatedDate { get; set; }

        public long TargetId { get; set; }
    }
}
