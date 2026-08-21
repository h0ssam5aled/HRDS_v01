using System;
using System.Collections.Generic;

namespace HRDS.Web.Models.Entities;

public partial class SafetyIncident
{
    public int IncidentId { get; set; }

    public int EmployeeId { get; set; }

    public int SafetyTypeId { get; set; }

    public DateTime IncidentDate { get; set; }

    public string? Location { get; set; }

    public string? Description { get; set; }

    public string? InjuryType { get; set; }

    public decimal? DaysLost { get; set; }

    public decimal? CostImpact { get; set; }

    public string? ActionTaken { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Employee Employee { get; set; } = null!;

    public virtual SafetyType SafetyType { get; set; } = null!;
}
