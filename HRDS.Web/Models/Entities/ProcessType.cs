using System;
using System.Collections.Generic;

namespace HRDS.Web.Models.Entities;

public partial class ProcessType
{
    public int ProcessTypeId { get; set; }

    public string ProcessCode { get; set; } = null!;

    public string ProcessNameAr { get; set; } = null!;

    public string? ProcessNameEn { get; set; }

    public bool IsActive { get; set; }

    public virtual ICollection<WorkflowTemplate> WorkflowTemplates { get; set; } = new List<WorkflowTemplate>();
}
