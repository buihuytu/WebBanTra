using System;
using System.Collections.Generic;

namespace WebBanTra.API.Models;

public partial class TblSlider
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Url { get; set; }

    public string? Description { get; set; }

    public int? Position { get; set; }

    public string? Image { get; set; }

    public DateTime? CreatedDate { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? UpdatedBy { get; set; }

    public int? IsDelete { get; set; }

    public int? IsActive { get; set; }
}
