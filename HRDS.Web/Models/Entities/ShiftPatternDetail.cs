using System;
using System.Collections.Generic;

namespace HRDS.Web.Models.Entities;

public partial class ShiftPatternDetail
{
    public int PatternDetailId { get; set; }

    public int PatternId { get; set; }

    public int? ShiftId { get; set; }

    public short DayNumber { get; set; }

    public bool IsOffDay { get; set; }

    public string? Remarks { get; set; }

    public string? Description { get; set; }

    public bool IsActive { get; set; }

    public int? CompanyId { get; set; }

    public int? CompanyBranchId { get; set; }

    public virtual ShiftPattern Pattern { get; set; } = null!;

    public virtual Shift? Shift { get; set; }
}
