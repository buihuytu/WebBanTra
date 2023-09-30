using System;
using System.Collections.Generic;

namespace WebBanTra.API.Models;

public partial class TblProductImage
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public int? IdProduct { get; set; }

    public virtual TblProduct? IdProductNavigation { get; set; }
}
