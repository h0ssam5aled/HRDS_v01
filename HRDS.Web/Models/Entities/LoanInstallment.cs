using System;
using System.Collections.Generic;

namespace HRDS.Web.Models.Entities;

public partial class LoanInstallment
{
    public int InstallmentId { get; set; }

    public int LoanId { get; set; }

    public int InstallmentNumber { get; set; }

    public DateOnly DueDate { get; set; }

    public decimal Amount { get; set; }

    public bool IsPaid { get; set; }

    public DateOnly? PaidDate { get; set; }

    public int? PayrollRunId { get; set; }

    public string? Notes { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Loan Loan { get; set; } = null!;

    public virtual PayrollRun? PayrollRun { get; set; }
}
