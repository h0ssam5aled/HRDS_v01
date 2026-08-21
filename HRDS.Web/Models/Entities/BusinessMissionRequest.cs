using System;
using System.Collections.Generic;

namespace HRDS.Web.Models.Entities;

public partial class BusinessMissionRequest
{
    public int MissionRequestId { get; set; }

    public int EmployeeId { get; set; }

    public int MissionTypeId { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }

    public TimeOnly? StartTime { get; set; }

    public TimeOnly? EndTime { get; set; }

    public string? Destination { get; set; }

    public string? Purpose { get; set; }

    public decimal? EstimatedAllowance { get; set; }

    public string? AttachmentPath { get; set; }

    public int OverallStatusId { get; set; }

    public bool IsActive { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? DeletedBy { get; set; }

    public DateTime? DeletedAt { get; set; }

    public bool IsDeleted { get; set; }

    public virtual ICollection<BusinessMissionExpense> BusinessMissionExpenses { get; set; } = new List<BusinessMissionExpense>();

    public virtual Employee Employee { get; set; } = null!;

    public virtual BusinessMissionType MissionType { get; set; } = null!;

    public virtual RequestStatus OverallStatus { get; set; } = null!;
}
