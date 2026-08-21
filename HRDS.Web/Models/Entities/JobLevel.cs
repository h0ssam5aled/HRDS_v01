using System;
using System.Collections.Generic;

namespace HRDS.Web.Models.Entities;

public partial class JobLevel
{
    public int JobLevelId { get; set; }

    public string JobLevelCode { get; set; } = null!;

    public string JobLevelNameAr { get; set; } = null!;

    public string? JobLevelNameEn { get; set; }

    public int? CompanyId { get; set; }

    public int? CompanyBranchId { get; set; }

    public bool IsActive { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? DeletedBy { get; set; }

    public DateTime? DeletedAt { get; set; }

    public bool IsDeleted { get; set; }

    public virtual ICollection<EmploymentHistory> EmploymentHistories { get; set; } = new List<EmploymentHistory>();

    public virtual ICollection<Position> Positions { get; set; } = new List<Position>();
}
