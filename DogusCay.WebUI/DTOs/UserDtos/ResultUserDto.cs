
namespace DogusCay.WebUI.DTOs.UserDtos
{
    public class ResultUserDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? ImageUrl { get; set; }
        public IList<string> Roles { get; set; }
        public string? RegionName { get; set; } // Yeni alan
        //public string FirstName { get; set; }
        //public string LastName { get; set; }
        //public string UserName { get; set; }
        //public string Email { get; set; }
        //public string? ImageUrl { get; set; }
        //public string RoleName { get; set; } 
    }
}
