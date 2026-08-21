using System;
using System.Collections.Generic;

namespace HRDS.Web.Models.Entities;

public partial class Religion
{
    public int ReligionId { get; set; }

    public string ReligionCode { get; set; } = null!;

    public string ReligionNameAr { get; set; } = null!;

    public string ReligionNameEn { get; set; } = null!;

    public bool IsActive { get; set; }

    public int SortOrder { get; set; }
}
