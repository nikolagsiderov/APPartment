using System.ComponentModel.DataAnnotations;

namespace APPartment.Models
{
    public class House : Base.Object
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required(ErrorMessage = "House password is required.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Please confirm your password.")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
