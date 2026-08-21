using System;
using System.Collections.Generic;

namespace HRDS.Web.Models.Entities;

public partial class LeaveRequest
{
    public int LeaveRequestId { get; set; }

    public int EmployeeId { get; set; }

    public int LeaveTypeId { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }

    public decimal TotalDays { get; set; }

    public int? SubstituteEmployeeId { get; set; }

    public string? Reason { get; set; }

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

    public virtual Employee Employee { get; set; } = null!;

    public virtual ICollection<LeaveRequestApproval> LeaveRequestApprovals { get; set; } = new List<LeaveRequestApproval>();

    public virtual ICollection<LeaveRequestAttachment> LeaveRequestAttachments { get; set; } = new List<LeaveRequestAttachment>();

    public virtual LeaveType LeaveType { get; set; } = null!;

    public virtual RequestStatus OverallStatus { get; set; } = null!;

    public virtual Employee? SubstituteEmployee { get; set; }
}
