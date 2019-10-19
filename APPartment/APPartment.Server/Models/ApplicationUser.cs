using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace APPartment.Server.Models
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        [ForeignKey("dbo.House")]
        public long HouseId { get; set; }
    }
}
