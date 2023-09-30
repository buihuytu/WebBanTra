using Microsoft.AspNetCore.Http;

namespace WebBanTra.API.Models
{
    public class SliderImage
    {
        public string? Name { get; set; }

        public string? Description { get; set; }

        public int? Position { get; set; }

        public int? CreatedBy { get; set; }

        public int? UpdatedBy { get; set; }

        public int? IsActive { get; set; }

        public IFormFile? FileImage { get; set; }
    }
}
