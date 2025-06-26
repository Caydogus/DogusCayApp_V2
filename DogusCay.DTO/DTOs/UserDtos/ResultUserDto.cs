using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogusCay.DTO.DTOs.UserDtos
{
    public class ResultUserDto
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? ImageUrl { get; set; }
        public IList<string> Roles { get; set; }
        public string? RegionName { get; set; } // Yeni alan
    }
}
