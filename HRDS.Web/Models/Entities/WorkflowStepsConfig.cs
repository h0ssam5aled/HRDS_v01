using System;
using System.Collections.Generic;

namespace HRDS.Web.Models.Entities;

public partial class WorkflowStepsConfig
{
    public int StepConfigId { get; set; }

    public int WorkflowTemplateId { get; set; }

    public int StepOrder { get; set; }

    public int ApproverTypeId { get; set; }

    public int? SpecificPositionId { get; set; }

    public int? SpecificEmployeeId { get; set; }

    public bool CanDelegate { get; set; }

    public short? AutoApproveDays { get; set; }

    public bool IsActive { get; set; }

    public virtual ApproverType ApproverType { get; set; } = null!;

    public virtual Position? SpecificPosition { get; set; }

    public virtual WorkflowTemplate WorkflowTemplate { get; set; } = null!;
}
