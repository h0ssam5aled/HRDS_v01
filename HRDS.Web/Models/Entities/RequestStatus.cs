using System;
using System.Collections.Generic;

namespace HRDS.Web.Models.Entities;

public partial class RequestStatus
{
    public int StatusId { get; set; }

    public string StatusCode { get; set; } = null!;

    public string StatusNameAr { get; set; } = null!;

    public string? StatusNameEn { get; set; }

    public string? BadgeClass { get; set; }

    public bool IsFinal { get; set; }

    public bool IsActive { get; set; }

    public virtual ICollection<BusinessMissionRequest> BusinessMissionRequests { get; set; } = new List<BusinessMissionRequest>();

    public virtual ICollection<EmployeeOvertimeRequest> EmployeeOvertimeRequests { get; set; } = new List<EmployeeOvertimeRequest>();

    public virtual ICollection<EmployeeRemoteWorkRequest> EmployeeRemoteWorkRequests { get; set; } = new List<EmployeeRemoteWorkRequest>();

    public virtual ICollection<LeaveRequestApproval> LeaveRequestApprovals { get; set; } = new List<LeaveRequestApproval>();

    public virtual ICollection<LeaveRequest> LeaveRequests { get; set; } = new List<LeaveRequest>();

    public virtual ICollection<Loan> Loans { get; set; } = new List<Loan>();

    public virtual ICollection<PermissionRequest> PermissionRequests { get; set; } = new List<PermissionRequest>();
}
