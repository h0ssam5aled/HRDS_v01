using System;
using System.Collections.Generic;

namespace HRDS.Web.Models.Entities;

public partial class EmployeeBonuse
{
    public int BonusId { get; set; }

    public int EmployeeId { get; set; }

    public int BonusTypeId { get; set; }

    public decimal Amount { get; set; }

    public int? CurrencyId { get; set; }

    public DateOnly BonusDate { get; set; }

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

    public virtual BonusType BonusType { get; set; } = null!;

    public virtual Employee Employee { get; set; } = null!;

    public virtual PayrollRun? PayrollRun { get; set; }
}
