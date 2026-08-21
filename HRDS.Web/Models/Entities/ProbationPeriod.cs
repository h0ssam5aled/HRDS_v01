using System;
using System.Collections.Generic;

namespace HRDS.Web.Models.Entities;

public partial class ProbationPeriod
{
    public int ProbationId { get; set; }

    public int EmployeeId { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }

    public bool IsConfirmed { get; set; }

    public DateOnly? ConfirmationDate { get; set; }

    public int? DecisionBy { get; set; }

    public string? Notes { get; set; }

    public bool IsActive { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? DeletedBy { get; set; }

    public DateTime? DeletedAt { get; set; }

    public bool IsDeleted { get; set; }

    public virtual Employee Employee { get; set; } = null!;
}
