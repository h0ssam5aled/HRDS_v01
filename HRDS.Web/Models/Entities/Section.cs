using System;
using System.Collections.Generic;

namespace HRDS.Web.Models.Entities;

public partial class Section
{
    public int SectionId { get; set; }

    public string SectionCode { get; set; } = null!;

    public string SectionNameAr { get; set; } = null!;

    public string? SectionNameEn { get; set; }

    public int DepartmentId { get; set; }

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

    public virtual Department Department { get; set; } = null!;

    public virtual ICollection<Unit> Units { get; set; } = new List<Unit>();
}
