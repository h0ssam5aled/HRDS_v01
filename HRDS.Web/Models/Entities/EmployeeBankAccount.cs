using System;
using System.Collections.Generic;

namespace HRDS.Web.Models.Entities;

public partial class EmployeeBankAccount
{
    public int EmployeeBankId { get; set; }

    public int EmployeeId { get; set; }

    public int BankId { get; set; }

    public int? BranchId { get; set; }

    public string AccountNumber { get; set; } = null!;

    public int? EmployeeBankAccountTypeId { get; set; }

    public string? Iban { get; set; }

    public int? CurrencyId { get; set; }

    public bool IsPrimary { get; set; }

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
