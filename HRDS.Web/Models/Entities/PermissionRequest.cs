using System;
using System.Collections.Generic;

namespace HRDS.Web.Models.Entities;

public partial class PermissionRequest
{
    public int PermissionRequestId { get; set; }

    public int EmployeeId { get; set; }

    public int PermissionTypeId { get; set; }

    public DateOnly PermissionDate { get; set; }

    public TimeOnly StartTime { get; set; }

    public TimeOnly EndTime { get; set; }

    public string? Reason { get; set; }

    public int OverallStatusId { get; set; }

    public virtual Employee Employee { get; set; } = null!;

    public virtual RequestStatus OverallStatus { get; set; } = null!;

    public virtual PermissionType PermissionType { get; set; } = null!;
}
