using System;
using System.Collections.Generic;

namespace HRDS.Web.Models.Entities;

public partial class Account
{
    public int AccountId { get; set; }

    public int AccountTypeId { get; set; }

    public int? ParentAccountId { get; set; }

    public int? CurrencyId { get; set; }

    public string AccountCode { get; set; } = null!;

    public string AccountNameAr { get; set; } = null!;

    public string? AccountNameEn { get; set; }

    public byte AccountLevel { get; set; }

    public bool IsLeaf { get; set; }

    public string AccountNature { get; set; } = null!;

    public string? HierarchyPath { get; set; }

    public string? Description { get; set; }

    public bool IsActive { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public virtual AccountType AccountType { get; set; } = null!;

    public virtual Currency? Currency { get; set; }

    public virtual ICollection<Account> InverseParentAccount { get; set; } = new List<Account>();

    public virtual Account? ParentAccount { get; set; }
}
