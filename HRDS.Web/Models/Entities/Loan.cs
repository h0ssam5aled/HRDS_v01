using System;
using System.Collections.Generic;

namespace HRDS.Web.Models.Entities;

public partial class Loan
{
    public int LoanId { get; set; }

    public int EmployeeId { get; set; }

    public decimal LoanAmount { get; set; }

    public int NumberOfInstallments { get; set; }

    public decimal MonthlyInstallment { get; set; }

    public DateOnly StartDeductionDate { get; set; }

    public int StatusId { get; set; }

    public virtual Employee Employee { get; set; } = null!;

    public virtual ICollection<LoanInstallment> LoanInstallments { get; set; } = new List<LoanInstallment>();

    public virtual RequestStatus Status { get; set; } = null!;
}
