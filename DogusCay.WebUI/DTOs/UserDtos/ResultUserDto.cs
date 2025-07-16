
using DogusCay.Entity.Entities;

namespace DogusCay.WebUI.DTOs.UserDtos
{
    public class ResultUserDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? ImageUrl { get; set; }
        public IList<string> Roles { get; set; }
        public string? RegionName { get; set; } // Yeni alan

        public int UserId { get; set; }   // Distrubutor eklerken bolge mudurlerini getirmek için eklendi.liste halinde
      //  public AppUser AppUser { get; set; } 


    }
}
