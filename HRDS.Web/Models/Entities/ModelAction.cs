using System;
using System.Collections.Generic;

namespace HRDS.Web.Models.Entities;

public partial class ModelAction
{
    public int ModelActionId { get; set; }

    public int ModelId { get; set; }

    public int ActionId { get; set; }

    public bool IsActive { get; set; }

    public virtual Action Action { get; set; } = null!;

    public virtual Model Model { get; set; } = null!;
}
