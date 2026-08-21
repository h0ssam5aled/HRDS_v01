using System;
using System.Collections.Generic;

namespace HRDS.Web.Models.Entities;

public partial class EmployeePenalty
{
    public int PenaltyId { get; set; }

    public int EmployeeId { get; set; }

    public int PenaltyTypeId { get; set; }

    public decimal? DeductionDays { get; set; }

    public decimal? DeductionAmount { get; set; }

    public DateOnly PenaltyDate { get; set; }

    public string? Reason { get; set; }

    public int? PayrollRunId { get; set; }

    public bool IsApproved { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? DeletedBy { get; set; }

    public DateTime? DeletedAt { get; set; }

    public bool IsDeleted { get; set; }

    public virtual Employee Employee { get; set; } = null!;

    public virtual PayrollRun? PayrollRun { get; set; }

    public virtual PenaltyType PenaltyType { get; set; } = null!;
}
