using System;
using System.Collections.Generic;

namespace HRDS.Web.Models.Entities;

public partial class EmployeeWorkSchedule
{
    public int EmployeeWorkScheduleId { get; set; }

    public int EmployeeId { get; set; }

    public int? ShiftId { get; set; }

    public int? PatternId { get; set; }

    public byte ScheduleType { get; set; }

    public DateOnly EffectiveFrom { get; set; }

    public DateOnly? EffectiveTo { get; set; }

    public byte? Priority { get; set; }

    public string? Remarks { get; set; }

    public bool IsActive { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? DeletedBy { get; set; }

    public DateTime? DeletedAt { get; set; }

    public bool IsDeleted { get; set; }

    public virtual Employee Employee { get; set; } = null!;

    public virtual ShiftPattern? Pattern { get; set; }

    public virtual Shift? Shift { get; set; }
}
