using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace DogusCay.Entity.Entities
{
    public class AppRole:IdentityRole<int>
    {
      
        //public int RoleId { get; set; }

        //[Required]
        //[MaxLength(50)]
        //public string RoleName { get; set; } // Örn: GenelMudur, BolgeMudur, SatisTemsilcisi

        //public ICollection<AppUser> Users { get; set; } 
    }

}
