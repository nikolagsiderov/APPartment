using System;
using System.ComponentModel.DataAnnotations;

namespace APPartment.Models
{
    public class History : Base.Object
    {
        [Key]
        public long Id { get; set; }

        public int FunctionTypeId { get; set; }

        public string ColumnName { get; set; }

        public string OldValue { get; set; }

        public string NewValue { get; set; }

        public string DeletedObjectName { get; set; }

        public long? DeletedObjectObjectType { get; set; }

        public DateTime? DeletedObjectDate { get; set; }

        public DateTime When { get; set; }

        public long HouseId { get; set; }

        public long? UserId { get; set; }

        public long? TargetId { get; set; }
    }
}
