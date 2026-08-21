using System;
using System.Collections.Generic;

namespace HRDS.Web.Models.Entities;

public partial class ShiftBreak
{
    public int BreakId { get; set; }

    public int ShiftId { get; set; }

    public string BreakCode { get; set; } = null!;

    public string BreakNameAr { get; set; } = null!;

    public string? BreakNameEn { get; set; }

    public TimeOnly FromTime { get; set; }

    public TimeOnly ToTime { get; set; }

    public bool IsPaidBreak { get; set; }

    public string? Description { get; set; }

    public bool IsActive { get; set; }

    public int? CompanyId { get; set; }

    public int? CompanyBranchId { get; set; }

    public virtual Shift Shift { get; set; } = null!;
}
