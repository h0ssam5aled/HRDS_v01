using System;
using System.Collections.Generic;

namespace HRDS.Web.Models.Entities;

public partial class Nationality
{
    public int NationalityId { get; set; }

    public string NationalityCode { get; set; } = null!;

    public string NationalityNameAr { get; set; } = null!;

    public string NationalityNameEn { get; set; } = null!;

    public bool IsActive { get; set; }

    public int SortOrder { get; set; }
}
