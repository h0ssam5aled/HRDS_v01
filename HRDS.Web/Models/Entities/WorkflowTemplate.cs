using System;
using System.Collections.Generic;

namespace HRDS.Web.Models.Entities;

public partial class WorkflowTemplate
{
    public int TemplateId { get; set; }

    public int ProcessTypeId { get; set; }

    public string TemplateCode { get; set; } = null!;

    public string TemplateNameAr { get; set; } = null!;

    public string? TemplateNameEn { get; set; }

    public bool IsActive { get; set; }

    public virtual ProcessType ProcessType { get; set; } = null!;

    public virtual ICollection<WorkflowStepsConfig> WorkflowStepsConfigs { get; set; } = new List<WorkflowStepsConfig>();
}
