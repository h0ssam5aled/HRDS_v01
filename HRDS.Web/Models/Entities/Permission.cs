using System;
using System.Collections.Generic;

namespace HRDS.Web.Models.Entities;

public partial class Permission
{
    public int PermissionId { get; set; }

    public int ModelId { get; set; }

    public int ActionId { get; set; }

    public string PermissionCode { get; set; } = null!;

    public string PermissionNameAr { get; set; } = null!;

    public string PermissionNameEn { get; set; } = null!;

    public string? Description { get; set; }

    public bool IsActive { get; set; }

    public virtual Action Action { get; set; } = null!;

    public virtual Model Model { get; set; } = null!;

    public virtual ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
}
