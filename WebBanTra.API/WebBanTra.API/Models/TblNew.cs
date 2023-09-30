using System;
using System.Collections.Generic;

namespace WebBanTra.API.Models;

public partial class TblNew
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Detail { get; set; }

    public string? Image { get; set; }

    public string? Slug { get; set; }

    public string? MetaTitle { get; set; }

    public string? MetaKey { get; set; }

    public string? MetaDesc { get; set; }

    public DateTime? CreatedDate { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? UpdatedBy { get; set; }

    public int? IsDelete { get; set; }

    public int? IsActive { get; set; }

    public string? Summary { get; set; }
}
