using System;
using System.Collections.Generic;

namespace HRDS.Web.Models.Entities;

public partial class Module
{
    public int ModuleId { get; set; }

    public string ModuleCode { get; set; } = null!;

    public string ModuleNameAr { get; set; } = null!;

    public string ModuleNameEn { get; set; } = null!;

    public string? Description { get; set; }

    public bool IsActive { get; set; }

    public virtual ICollection<Model> Models { get; set; } = new List<Model>();
}
