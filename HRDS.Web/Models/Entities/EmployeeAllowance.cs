using System;
using System.Collections.Generic;

namespace HRDS.Web.Models.Entities;

public partial class EmployeeAllowance
{
    public int EmployeeAllowanceId { get; set; }

    public int EmployeeId { get; set; }

    public int AllowanceTypeId { get; set; }

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

    public virtual AllowanceType AllowanceType { get; set; } = null!;

    public virtual Employee Employee { get; set; } = null!;
}
