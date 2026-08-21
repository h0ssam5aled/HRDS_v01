using System;
using System.Collections.Generic;

namespace HRDS.Web.Models.Entities;

public partial class EmploymentHistory
{
    public int HistoryId { get; set; }

    public int EmployeeId { get; set; }

    public int? DirectManagerId { get; set; }

    public int EmployeeStatusId { get; set; }

    public int DepartmentId { get; set; }

    public int? SectionId { get; set; }

    public int JobTitleId { get; set; }

    public int? JobLevelId { get; set; }

    public int? CostCenterId { get; set; }

    public int EmploymentTypeId { get; set; }

    public DateOnly HireDate { get; set; }

    public DateOnly? TerminationDate { get; set; }

    public string? ResonOfLeaving { get; set; }

    public bool IsActive { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? DeletedBy { get; set; }

    public DateTime? DeletedAt { get; set; }

    public bool IsDeleted { get; set; }

    public int? CompanyId { get; set; }

    public int? CompanyBranchId { get; set; }

    public virtual Department Department { get; set; } = null!;

    public virtual Employee? DirectManager { get; set; }

    public virtual Employee Employee { get; set; } = null!;

    public virtual EmployeeStatus EmployeeStatus { get; set; } = null!;

    public virtual EmploymentType EmploymentType { get; set; } = null!;

    public virtual JobLevel? JobLevel { get; set; }

    public virtual JobTitle JobTitle { get; set; } = null!;
}
