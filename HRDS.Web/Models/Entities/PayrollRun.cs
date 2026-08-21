using System;
using System.Collections.Generic;

namespace HRDS.Web.Models.Entities;

public partial class PayrollRun
{
    public int PayrollRunId { get; set; }

    public int? CompanyId { get; set; }

    public int Year { get; set; }

    public int Month { get; set; }

    public bool IsClosed { get; set; }

    public decimal TotalBasicSalary { get; set; }

    public decimal TotalAllowances { get; set; }

    public decimal TotalDeductions { get; set; }

    public decimal NetSalary { get; set; }

    public bool IsActive { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? DeletedBy { get; set; }

    public DateTime? DeletedAt { get; set; }

    public bool IsDeleted { get; set; }

    public virtual ICollection<EmployeeBonuse> EmployeeBonuses { get; set; } = new List<EmployeeBonuse>();

    public virtual ICollection<EmployeePenalty> EmployeePenalties { get; set; } = new List<EmployeePenalty>();

    public virtual ICollection<EmployeeSalaryDetail> EmployeeSalaryDetails { get; set; } = new List<EmployeeSalaryDetail>();

    public virtual ICollection<LoanInstallment> LoanInstallments { get; set; } = new List<LoanInstallment>();
}
