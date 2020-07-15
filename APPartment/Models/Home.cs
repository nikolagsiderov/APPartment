using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APPartment.Models
{
    public class Home : Base.Object
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required(ErrorMessage = "Home password is required.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Please confirm your password.")]
        [DataType(DataType.Password)]

        [NotMapped]
        public string ConfirmPassword { get; set; }
    }
}
