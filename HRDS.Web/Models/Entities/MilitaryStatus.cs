using System;
using System.Collections.Generic;

namespace HRDS.Web.Models.Entities;

public partial class MilitaryStatus
{
    public int MilitaryStatusId { get; set; }

    public string MilitaryStatusCode { get; set; } = null!;

    public string MilitaryStatusNameAr { get; set; } = null!;

    public string MilitaryStatusNameEn { get; set; } = null!;

    public bool IsActive { get; set; }

    public int SortOrder { get; set; }
}
