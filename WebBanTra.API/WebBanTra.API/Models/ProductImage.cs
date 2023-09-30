namespace WebBanTra.API.Models
{
    public class ProductImage
    {
        public string? Name { get; set; }

        public int? CateId { get; set; }

        public int? Mass { get; set; }

        public int? Price { get; set; }

        public int? ProPrice { get; set; }

        public string? Description { get; set; }

        public string? Detail { get; set; }

        public string? MetaTitle { get; set; }

        public string? MetaKey { get; set; }

        public string? MetaDesc { get; set; }

        public int? CreatedBy { get; set; }

        public int? UpdatedBy { get; set; }

        public int? IsActive { get; set; }

        public IFormFile? FileImage { get; set; }
    }
}
