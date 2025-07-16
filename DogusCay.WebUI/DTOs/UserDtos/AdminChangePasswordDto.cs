namespace DogusCay.WebUI.DTOs.UserDtos
{
    public class AdminChangePasswordDto
    {
        public int UserId { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
