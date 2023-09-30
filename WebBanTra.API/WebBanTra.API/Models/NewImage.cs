using Microsoft.AspNetCore.Http;

namespace WebBanTra.API.Models
{
    public class NewImage
    {
        public string? Name { get; set; }

        public string? Detail { get; set; }

        public string? Summary { get; set; }

        public string? MetaTitle { get; set; }

        public string? MetaKey { get; set; }

        public string? MetaDesc { get; set; }

        public int? CreatedBy { get; set; }

        public int? UpdatedBy { get; set; }

        public int? IsActive { get; set; }

        public IFormFile? FileImage { get; set; } 
    }
}
