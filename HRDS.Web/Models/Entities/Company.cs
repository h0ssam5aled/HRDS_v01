using System;
using System.Collections.Generic;

namespace HRDS.Web.Models.Entities;

public partial class Company
{
    public int CompanyId { get; set; }

    public string CompanyCode { get; set; } = null!;

    public string CompanyNameAr { get; set; } = null!;

    public string CompanyNameEn { get; set; } = null!;

    public string? TaxNumber { get; set; }

    public string? CommercialRegister { get; set; }

    public int CountryId { get; set; }

    public int GovernorateId { get; set; }

    public int CityId { get; set; }

    public string? Address { get; set; }

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public string? Website { get; set; }

    public bool IsActive { get; set; }

    public virtual City City { get; set; } = null!;

    public virtual CompanyBranch? CompanyBranch { get; set; }

    public virtual Governorate Governorate { get; set; } = null!;

    public virtual ICollection<UserAccess> UserAccesses { get; set; } = new List<UserAccess>();
}
