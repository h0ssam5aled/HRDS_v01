using System;
using System.Collections.Generic;

namespace HRDS.Web.Models.Entities;

public partial class Model
{
    public int ModelId { get; set; }

    public int ModuleId { get; set; }

    public string ModelCode { get; set; } = null!;

    public string ModelNameAr { get; set; } = null!;

    public string ModelNameEn { get; set; } = null!;

    public string? Description { get; set; }

    public bool IsActive { get; set; }

    public virtual ICollection<ModelAction> ModelActions { get; set; } = new List<ModelAction>();

    public virtual Module Module { get; set; } = null!;

    public virtual ICollection<Permission> Permissions { get; set; } = new List<Permission>();
}
