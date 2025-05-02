using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace DogusCay.Entity.Entities
{
    public class AppUser : IdentityUser<int>
    {
      
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? ImageUrl { get; set; }

        // Navigasyon: Bu müdürün yaptığı satışlar (kendi satış da yapabilir sistemde)
    
        

        //public Region Region { get; set; }
        //// Bağlı olduğu bölge
        //public int RegionId { get; set; }
        //// Rol bilgisi (örn: Müdür, Genel Müdür)
        //public int RoleId { get; set; }
        //public Role Role { get; set; }

    }
}
