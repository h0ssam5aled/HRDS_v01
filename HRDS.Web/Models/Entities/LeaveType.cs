using System;
using System.Collections.Generic;

namespace HRDS.Web.Models.Entities;

public partial class LeaveType
{
    public int LeaveTypeId { get; set; }

    public string LeaveCode { get; set; } = null!;

    public string LeaveNameAr { get; set; } = null!;

    public string? LeaveNameEn { get; set; }

    public bool IsPaid { get; set; }

    public bool RequiresBalance { get; set; }

    public bool RequiresAttachment { get; set; }

    public bool RequiresApproval { get; set; }

    public bool AllowCarryForward { get; set; }

    public decimal? CarryForwardLimit { get; set; }

    public decimal? MaxDaysPerRequest { get; set; }

    public decimal? MaxDaysPerYear { get; set; }

    public bool AllowHalfDay { get; set; }

    public bool IncludePublicHolidays { get; set; }

    public bool IncludeWeekends { get; set; }

    public short? MinimumServiceMonths { get; set; }

    public bool CanExceedBalance { get; set; }

    public bool RequiresWorkflow { get; set; }

    public bool AllowBackDateRequest { get; set; }

    public bool AllowFutureRequest { get; set; }

    public short? MaxFutureDays { get; set; }

    public byte? GenderRestriction { get; set; }

    public bool DeductFromSalary { get; set; }

    public int? LeaveCategoryId { get; set; }

    public bool RequiresComment { get; set; }

    public bool AllowHourlyLeave { get; set; }

    public decimal? MinimumDaysPerRequest { get; set; }

    public decimal? MaximumConsecutiveDays { get; set; }

    public short? GapBetweenRequestsDays { get; set; }

    public bool AllowDuringProbation { get; set; }

    public bool AutoApprove { get; set; }

    public bool IsEncashable { get; set; }

    public decimal? EncashmentLimit { get; set; }

    public bool ExpireAtYearEnd { get; set; }

    public short? DisplayOrder { get; set; }

    public string? ColorCode { get; set; }

    public string? IconName { get; set; }

    public bool IsActive { get; set; }

    public virtual ICollection<EmployeeLeaveBalance> EmployeeLeaveBalances { get; set; } = new List<EmployeeLeaveBalance>();

    public virtual ICollection<LeaveRequest> LeaveRequests { get; set; } = new List<LeaveRequest>();
}
