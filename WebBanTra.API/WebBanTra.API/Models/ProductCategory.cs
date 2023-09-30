namespace WebBanTra.API.Models
{
    public class ProductCategory
    {
        public int ProductId { get; set; }
        public string? ProductName { get; set; }

        public string? Slug { get; set; }

        public string? ProductImg { get; set; }
        public string? CategoryName { get; set;}
        public int? ProductPrice { get; set; }
        public int? IsActive { get; set; }

    }
}
