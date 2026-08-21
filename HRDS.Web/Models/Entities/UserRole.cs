using System;
using System.Collections.Generic;

namespace HRDS.Web.Models.Entities;

public partial class UserRole
{
    public int UserAccessId { get; set; }

    public int RoleId { get; set; }

    public DateTime AssignedAt { get; set; }

    public bool IsActive { get; set; }

    public virtual Role Role { get; set; } = null!;

    public virtual UserAccess UserAccess { get; set; } = null!;
}
