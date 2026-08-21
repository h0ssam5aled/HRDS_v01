using System;
using System.Collections.Generic;

namespace HRDS.Web.Models.Entities;

public partial class PenaltyType
{
    public int PenaltyTypeId { get; set; }

    public string PenaltyTypeCode { get; set; } = null!;

    public string PenaltyTypeNameAr { get; set; } = null!;

    public string? PenaltyTypeNameEn { get; set; }

    public decimal? DefaultDeductionDays { get; set; }

    public bool IsActive { get; set; }

    public virtual ICollection<EmployeePenalty> EmployeePenalties { get; set; } = new List<EmployeePenalty>();
}
