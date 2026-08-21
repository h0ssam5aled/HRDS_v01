using System;
using System.Collections.Generic;

namespace HRDS.Web.Models.Entities;

public partial class Governorate
{
    public int GovernorateId { get; set; }

    public int CountryId { get; set; }

    public string GovernorateCode { get; set; } = null!;

    public string GovernorateNameAr { get; set; } = null!;

    public string GovernorateNameEn { get; set; } = null!;

    public bool IsActive { get; set; }

    public int SortOrder { get; set; }

    public virtual ICollection<City> Cities { get; set; } = new List<City>();

    public virtual ICollection<Company> Companies { get; set; } = new List<Company>();

    public virtual ICollection<CompanyBranch> CompanyBranches { get; set; } = new List<CompanyBranch>();

    public virtual Country Country { get; set; } = null!;
}
