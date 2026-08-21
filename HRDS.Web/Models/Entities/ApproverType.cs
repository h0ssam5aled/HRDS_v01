using System;
using System.Collections.Generic;

namespace HRDS.Web.Models.Entities;

public partial class ApproverType
{
    public int ApproverTypeId { get; set; }

    public string ApproverTypeCode { get; set; } = null!;

    public string ApproverTypeNameAr { get; set; } = null!;

    public string? ApproverTypeNameEn { get; set; }

    public bool RequiresJobTitle { get; set; }

    public bool RequiresEmployee { get; set; }

    public bool IsActive { get; set; }

    public virtual ICollection<WorkflowStepsConfig> WorkflowStepsConfigs { get; set; } = new List<WorkflowStepsConfig>();
}
