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
        public ICollection<Point> Points { get; set; }
        // Bu kullanıcının yaptığı satışlar (Satış Temsilcisi ise)
        public ICollection<Sale> Sales { get; set; }

    }
}
