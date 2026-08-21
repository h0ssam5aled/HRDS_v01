using System;
using System.Collections.Generic;

namespace HRDS.Web.Models.Entities;

public partial class JobGroup
{
    public int JobGroupId { get; set; }

    public string JobGroupCode { get; set; } = null!;

    public string JobGroupNameAr { get; set; } = null!;

    public string? JobGroupNameEn { get; set; }

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

    public virtual ICollection<JobTitle> JobTitles { get; set; } = new List<JobTitle>();
}
