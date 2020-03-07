using System.ComponentModel.DataAnnotations;

namespace APPartment.Models
{
    public class Comment : Base.Object
    {
        [Key]
        public long Id { get; set; }

        public string Text { get; set; }

        public string Username { get; set; }

        public long TargetId { get; set; }
    }
}
