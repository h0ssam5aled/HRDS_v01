using System;
using System.Collections.Generic;

namespace HRDS.Web.Models.Entities;

public partial class ProbationStatus
{
    public int ProbationStatusId { get; set; }

    public string StatusCode { get; set; } = null!;

    public string StatusNameAr { get; set; } = null!;

    public string? StatusNameEn { get; set; }

    public string? Description { get; set; }
}
