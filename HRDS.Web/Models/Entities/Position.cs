using System;
using System.Collections.Generic;

namespace HRDS.Web.Models.Entities;

public partial class Position
{
    public int PositionId { get; set; }

    public int? UnitId { get; set; }

    public int? ParentPositionId { get; set; }

    public int? ReportsToPositionId { get; set; }

    public int JobTitleId { get; set; }

    public int? JobLevelId { get; set; }

    public int PositionStatusId { get; set; }

    public string PositionCode { get; set; } = null!;

    public string PositionNameAr { get; set; } = null!;

    public string? PositionNameEn { get; set; }

    public short? HeadCount { get; set; }

    public bool IsManagerial { get; set; }

    public DateOnly? EffectiveFrom { get; set; }

    public DateOnly? EffectiveTo { get; set; }

    public string? Remarks { get; set; }

    public bool IsActive { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? DeletedBy { get; set; }

    public DateTime? DeletedAt { get; set; }

    public bool IsDeleted { get; set; }

    public virtual ICollection<EmployeePosition> EmployeePositions { get; set; } = new List<EmployeePosition>();

    public virtual ICollection<Position> InverseParentPosition { get; set; } = new List<Position>();

    public virtual ICollection<Position> InverseReportsToPosition { get; set; } = new List<Position>();

    public virtual JobLevel? JobLevel { get; set; }

    public virtual JobTitle JobTitle { get; set; } = null!;

    public virtual ICollection<OrganizationTree> OrganizationTreeAncestorPositions { get; set; } = new List<OrganizationTree>();

    public virtual ICollection<OrganizationTree> OrganizationTreeDescendantPositions { get; set; } = new List<OrganizationTree>();

    public virtual Position? ParentPosition { get; set; }

    public virtual PositionStatus PositionStatus { get; set; } = null!;

    public virtual Position? ReportsToPosition { get; set; }

    public virtual Unit? Unit { get; set; }

    public virtual ICollection<WorkflowStepsConfig> WorkflowStepsConfigs { get; set; } = new List<WorkflowStepsConfig>();
}
