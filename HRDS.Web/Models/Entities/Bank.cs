using System;
using System.Collections.Generic;

namespace HRDS.Web.Models.Entities;

public partial class Bank
{
    public int BankId { get; set; }

    public string BankCode { get; set; } = null!;

    public string BankNameAr { get; set; } = null!;

    public string? BankNameEn { get; set; }

    public string? SwiftCode { get; set; }

    public string? Description { get; set; }

    public bool IsActive { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public virtual ICollection<BankBranch> BankBranches { get; set; } = new List<BankBranch>();
}
