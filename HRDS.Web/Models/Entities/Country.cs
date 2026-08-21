using System;
using System.Collections.Generic;

namespace HRDS.Web.Models.Entities;

public partial class Country
{
    public int CountryId { get; set; }

    public string CountryCode2 { get; set; } = null!;

    public string CountryCode3 { get; set; } = null!;

    public string CountryNameAr { get; set; } = null!;

    public string CountryNameEn { get; set; } = null!;

    public bool IsActive { get; set; }

    public int SortOrder { get; set; }

    public virtual ICollection<Governorate> Governorates { get; set; } = new List<Governorate>();
}
