using System;
using System.Collections.Generic;

namespace HRDS.Web.Models.Entities;

public partial class AssignmentReason
{
    public int AssignmentReasonId { get; set; }

    public string AssignmentReasonCode { get; set; } = null!;

    public string AssignmentReasonNameAr { get; set; } = null!;

    public string? AssignmentReasonNameEn { get; set; }

    public string? Description { get; set; }

    public bool IsActive { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? DeletedBy { get; set; }

    public DateTime? DeletedAt { get; set; }

    public bool IsDeleted { get; set; }

    public virtual ICollection<EmployeePosition> EmployeePositions { get; set; } = new List<EmployeePosition>();
}
