using System;
using System.Collections.Generic;

namespace HRDS.Web.Models.Entities;

public partial class CompanyBranch
{
    public int CompanyBranchId { get; set; }

    public int CompanyId { get; set; }

    public int CountryId { get; set; }

    public int GovernorateId { get; set; }

    public int CityId { get; set; }

    public string BranchCode { get; set; } = null!;

    public string BranchNameAr { get; set; } = null!;

    public string BranchNameEn { get; set; } = null!;

    public string? Address { get; set; }

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public bool IsMainBranch { get; set; }

    public bool IsActive { get; set; }

    public virtual City City { get; set; } = null!;

    public virtual Company Company { get; set; } = null!;

    public virtual Governorate Governorate { get; set; } = null!;

    public virtual ICollection<UserAccess> UserAccesses { get; set; } = new List<UserAccess>();
}
