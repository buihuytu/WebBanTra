using System;
using System.Collections.Generic;

namespace WebBanTra.API.Models;

public partial class TblUser
{
    public int Id { get; set; }

    public string? FullName { get; set; }

    public string? UserName { get; set; }

    public string? Password { get; set; }

    public int? Role { get; set; }

    public string? Image { get; set; }

    public string? Address { get; set; }

    public string? Email { get; set; }

    public int? Phone { get; set; }

    public DateTime? CreatedDate { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? UpdatedBy { get; set; }

    public int? IsDelete { get; set; }

    public int? IsActive { get; set; }
}
