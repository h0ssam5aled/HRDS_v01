using System;
using System.Collections.Generic;

namespace HRDS.Web.Models.Entities;

public partial class ShiftPattern
{
    public int PatternId { get; set; }

    public string PatternCode { get; set; } = null!;

    public string PatternNameAr { get; set; } = null!;

    public string? PatternNameEn { get; set; }

    public short CycleDays { get; set; }

    public bool IsDefault { get; set; }

    public string? Description { get; set; }

    public bool IsActive { get; set; }

    public int? CompanyId { get; set; }

    public int? CompanyBranchId { get; set; }

    public virtual ICollection<EmployeeWorkSchedule> EmployeeWorkSchedules { get; set; } = new List<EmployeeWorkSchedule>();

    public virtual ICollection<ShiftPatternDetail> ShiftPatternDetails { get; set; } = new List<ShiftPatternDetail>();
}
