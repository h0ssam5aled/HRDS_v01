using System;
using System.Collections.Generic;

namespace HRDS.Web.Models.Entities;

public partial class EmploymentType
{
    public int EmploymentTypeId { get; set; }

    public string EmploymentTypeCode { get; set; } = null!;

    public string EmploymentTypeNameAr { get; set; } = null!;

    public string? EmploymentTypeNameEn { get; set; }

    public string? Description { get; set; }

    public decimal? DefaultWorkingHours { get; set; }

    public bool IsOvertimeAllowed { get; set; }

    public bool IsLeaveEligible { get; set; }

    public virtual ICollection<EmploymentHistory> EmploymentHistories { get; set; } = new List<EmploymentHistory>();
}
