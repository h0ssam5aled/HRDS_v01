using System;
using System.Collections.Generic;

namespace HRDS.Web.Models.Entities;

public partial class EmployeePosition
{
    public int EmployeePositionId { get; set; }

    public int EmployeeId { get; set; }

    public int PositionId { get; set; }

    public bool PrimaryPosition { get; set; }

    public DateOnly FromDate { get; set; }

    public DateOnly? ToDate { get; set; }

    public int? AssignmentReasonId { get; set; }

    public bool IsActive { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? DeletedBy { get; set; }

    public DateTime? DeletedAt { get; set; }

    public bool IsDeleted { get; set; }

    public virtual AssignmentReason? AssignmentReason { get; set; }

    public virtual Employee Employee { get; set; } = null!;

    public virtual Position Position { get; set; } = null!;
}
