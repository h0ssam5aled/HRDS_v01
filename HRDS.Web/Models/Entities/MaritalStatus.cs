using System;
using System.Collections.Generic;

namespace HRDS.Web.Models.Entities;

public partial class MaritalStatus
{
    public int MaritalStatusId { get; set; }

    public string MaritalStatusCode { get; set; } = null!;

    public string MaritalStatusNameAr { get; set; } = null!;

    public string MaritalStatusNameEn { get; set; } = null!;

    public bool IsActive { get; set; }

    public int SortOrder { get; set; }
}
