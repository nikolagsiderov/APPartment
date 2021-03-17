using APPartment.Data.Server.Models.Base;
using APPartment.ORM.Framework.Attributes;
using APPartment.UI.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APPartment.Data.Server.Models.Core
{
    [Table("User", Schema = "dbo")]
    public class User : IdentityBaseObject
    {
        [FieldMappingForMainTable]
        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        [APPUIHint(Templates.Password)]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Please confirm your password.")]
        [DataType(DataType.Password)]
        [APPUIHint(Templates.Password)]
        public string ConfirmPassword { get; set; }
    }
}
