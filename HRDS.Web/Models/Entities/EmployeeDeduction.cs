using System;
using System.Collections.Generic;

namespace HRDS.Web.Models.Entities;

public partial class EmployeeDeduction
{
    public int EmployeeDeductionId { get; set; }

    public int EmployeeId { get; set; }

    public int DeductionTypeId { get; set; }

    public decimal Amount { get; set; }

    public DateOnly FromDate { get; set; }

    public DateOnly? ToDate { get; set; }

    public string? Notes { get; set; }

    public bool IsActive { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? DeletedBy { get; set; }

    public DateTime? DeletedAt { get; set; }

    public bool IsDeleted { get; set; }

    public virtual DeductionType DeductionType { get; set; } = null!;

    public virtual Employee Employee { get; set; } = null!;
}
