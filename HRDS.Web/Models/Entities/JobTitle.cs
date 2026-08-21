using System;
using System.Collections.Generic;

namespace HRDS.Web.Models.Entities;

public partial class JobTitle
{
    public int JobTitleId { get; set; }

    public int JobGroupId { get; set; }

    public string JobTitleCode { get; set; } = null!;

    public string JobTitleNameAr { get; set; } = null!;

    public string? JobTitleNameEn { get; set; }

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

    public virtual JobGroup JobGroup { get; set; } = null!;

    public virtual ICollection<Position> Positions { get; set; } = new List<Position>();
}
