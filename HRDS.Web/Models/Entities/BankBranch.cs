using System;
using System.Collections.Generic;

namespace HRDS.Web.Models.Entities;

public partial class BankBranch
{
    public int BranchId { get; set; }

    public int BankId { get; set; }

    public string BankBranchCode { get; set; } = null!;

    public string BankBranchNameAr { get; set; } = null!;

    public string? BankBranchNameEn { get; set; }

    public string? BankBranchAddress { get; set; }

    public string? BankBranchPhone { get; set; }

    public string? Description { get; set; }

    public bool IsActive { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public virtual Bank Bank { get; set; } = null!;
}
