using System;
using System.Collections.Generic;

namespace HRDS.Web.Models.Entities;

public partial class Action
{
    public int ActionId { get; set; }

    public string ActionCode { get; set; } = null!;

    public string ActionNameAr { get; set; } = null!;

    public string ActionNameEn { get; set; } = null!;

    public string? Description { get; set; }

    public bool IsActive { get; set; }

    public virtual ICollection<ModelAction> ModelActions { get; set; } = new List<ModelAction>();

    public virtual ICollection<Permission> Permissions { get; set; } = new List<Permission>();
}
