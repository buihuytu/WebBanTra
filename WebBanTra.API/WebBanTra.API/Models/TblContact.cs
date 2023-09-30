using System;
using System.Collections.Generic;

namespace WebBanTra.API.Models;

public partial class TblContact
{
    public int Id { get; set; }

    public string? FullName { get; set; }

    public string? Email { get; set; }

    public int? Phone { get; set; }

    public string? Detail { get; set; }

    public int? IsReply { get; set; }

    public string? Reply { get; set; }

    public DateTime? CreatedDate { get; set; }

    public int? RepliedBy { get; set; }

    public DateTime? RepliedDate { get; set; }

    public int? IsDelete { get; set; }
}
