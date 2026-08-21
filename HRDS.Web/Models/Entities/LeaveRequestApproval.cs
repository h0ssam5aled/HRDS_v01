using System;
using System.Collections.Generic;

namespace HRDS.Web.Models.Entities;

public partial class LeaveRequestApproval
{
    public int ApprovalId { get; set; }

    public int LeaveRequestId { get; set; }

    public int StepOrder { get; set; }

    public int ApproverEmployeeId { get; set; }

    public int StatusId { get; set; }

    public DateTime? ActionDate { get; set; }

    public string? Comments { get; set; }

    public bool IsActive { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? DeletedBy { get; set; }

    public DateTime? DeletedAt { get; set; }

    public bool IsDeleted { get; set; }

    public virtual Employee ApproverEmployee { get; set; } = null!;

    public virtual LeaveRequest LeaveRequest { get; set; } = null!;

    public virtual RequestStatus Status { get; set; } = null!;
}
