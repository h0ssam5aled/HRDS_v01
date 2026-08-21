using System;
using System.Collections.Generic;

namespace HRDS.Web.Models.Entities;

public partial class EmployeeSalaryDetail
{
    public int SalaryDetailId { get; set; }

    public int PayrollRunId { get; set; }

    public int EmployeeId { get; set; }

    public decimal BasicSalary { get; set; }

    public decimal AllowancesAmount { get; set; }

    public decimal OvertimeAmount { get; set; }

    public decimal AbsenceDeduction { get; set; }

    public decimal DelayDeduction { get; set; }

    public decimal LoansDeduction { get; set; }

    public decimal SocialInsuranceEmployee { get; set; }

    public decimal TaxAmount { get; set; }

    public decimal NetSalary { get; set; }

    public bool IsActive { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? DeletedBy { get; set; }

    public DateTime? DeletedAt { get; set; }

    public bool IsDeleted { get; set; }

    public virtual Employee Employee { get; set; } = null!;

    public virtual PayrollRun PayrollRun { get; set; } = null!;
}
