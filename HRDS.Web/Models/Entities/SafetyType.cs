using System;
using System.Collections.Generic;

namespace HRDS.Web.Models.Entities;

public partial class SafetyType
{
    public int SafetyTypeId { get; set; }

    public string SafetyTypeCode { get; set; } = null!;

    public string SafetyTypeNameAr { get; set; } = null!;

    public string? SafetyTypeNameEn { get; set; }

    public byte? SeverityLevel { get; set; }

    public bool IsActive { get; set; }

    public virtual ICollection<SafetyIncident> SafetyIncidents { get; set; } = new List<SafetyIncident>();
}
