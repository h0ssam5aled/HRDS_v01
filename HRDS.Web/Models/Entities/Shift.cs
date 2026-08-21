using System;
using System.Collections.Generic;

namespace HRDS.Web.Models.Entities;

public partial class Shift
{
    public int ShiftId { get; set; }

    public int ShiftTypeId { get; set; }

    public string ShiftCode { get; set; } = null!;

    public string ShiftNameAr { get; set; } = null!;

    public string? ShiftNameEn { get; set; }

    public TimeOnly StartTime { get; set; }

    public TimeOnly EndTime { get; set; }

    public bool CrossMidnight { get; set; }

    public int GraceInMinutes { get; set; }

    public int GraceOutMinutes { get; set; }

    public bool AllowOvertime { get; set; }

    public int MinimumOvertimeMinutes { get; set; }

    public bool AllowLateDeduction { get; set; }

    public bool AutoCloseAttendance { get; set; }

    public decimal? StandardHours { get; set; }

    public bool IsActive { get; set; }

    public virtual ICollection<EmployeeWorkSchedule> EmployeeWorkSchedules { get; set; } = new List<EmployeeWorkSchedule>();

    public virtual ICollection<ShiftBreak> ShiftBreaks { get; set; } = new List<ShiftBreak>();

    public virtual ICollection<ShiftPatternDetail> ShiftPatternDetails { get; set; } = new List<ShiftPatternDetail>();

    public virtual ShiftType ShiftType { get; set; } = null!;
}
