using System;
using System.Collections.Generic;

namespace HRDS.Web.Models.Entities;

public partial class City
{
    public int CityId { get; set; }

    public int GovernorateId { get; set; }

    public string CityCode { get; set; } = null!;

    public string CityNameAr { get; set; } = null!;

    public string CityNameEn { get; set; } = null!;

    public bool IsActive { get; set; }

    public int SortOrder { get; set; }

    public virtual ICollection<Company> Companies { get; set; } = new List<Company>();

    public virtual ICollection<CompanyBranch> CompanyBranches { get; set; } = new List<CompanyBranch>();

    public virtual Governorate Governorate { get; set; } = null!;
}
