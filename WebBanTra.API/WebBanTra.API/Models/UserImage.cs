namespace WebBanTra.API.Models
{
    public class UserImage
    {
        public string? FullName { get; set; }

        public string? UserName { get; set; }

        public string? Password { get; set; }

        public int? Role { get; set; }

        public string? Address { get; set; }

        public string? Email { get; set; }

        public int? Phone { get; set; }

        public int? CreatedBy { get; set; }

        public int? UpdatedBy { get; set; }

        public int? IsActive { get; set; }

        public IFormFile? FileImage { get; set; }
    }
}
