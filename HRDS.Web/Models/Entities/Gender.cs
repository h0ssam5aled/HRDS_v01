using System;
using System.Collections.Generic;

namespace HRDS.Web.Models.Entities;

public partial class Gender
{
    public int GenderId { get; set; }

    public string GenderCode { get; set; } = null!;

    public string GenderNameAr { get; set; } = null!;

    public string GenderNameEn { get; set; } = null!;

    public bool IsActive { get; set; }

    public int SortOrder { get; set; }
}
