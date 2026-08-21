using System;
using System.Collections.Generic;

namespace HRDS.Web.Models.Entities;

public partial class OrganizationTree
{
    public int AncestorPositionId { get; set; }

    public int DescendantPositionId { get; set; }

    public int Depth { get; set; }

    public virtual Position AncestorPosition { get; set; } = null!;

    public virtual Position DescendantPosition { get; set; } = null!;
}
