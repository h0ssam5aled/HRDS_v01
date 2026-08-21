using System;
using System.Collections.Generic;

namespace HRDS.Web.Models.Entities;

public partial class BankAccountType
{
    public int BankAccountTypeId { get; set; }

    public string BankAccountTypeCode { get; set; } = null!;

    public string BankAccountTypeNameAr { get; set; } = null!;

    public string? BankAccountTypeNameEn { get; set; }

    public string? Description { get; set; }

    public bool IsActive { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }
}
