using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APPartment.Models
{
    public class History : Base.Object
    {
        public long Id { get; set; }

        public string Function { get; set; }

        public string ColumnName { get; set; }

        public string OldValue { get; set; }

        public string NewValue { get; set; }

        public long? TargetId { get; set; }
    }
}
