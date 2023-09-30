using System;
using System.Collections.Generic;

namespace WebBanTra.API.Models;

public partial class TblProduct
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Slug { get; set; }

    public int? CateId { get; set; }

    public int? Mass { get; set; }

    public int? Price { get; set; }

    public int? ProPrice { get; set; }

    public string? Description { get; set; }

    public string? Detail { get; set; }

    public string? Image { get; set; }

    public string? MetaTitle { get; set; }

    public string? MetaKey { get; set; }

    public string? MetaDesc { get; set; }

    public DateTime? CreatedDate { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? UpdatedBy { get; set; }

    public int? IsDelete { get; set; }

    public int? IsActive { get; set; }

    public virtual TblCategory? Cate { get; set; }

    public virtual ICollection<TblProductImage> TblProductImages { get; set; } = new List<TblProductImage>();
}
