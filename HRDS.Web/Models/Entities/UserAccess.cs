using System;
using System.Collections.Generic;

namespace HRDS.Web.Models.Entities;

public partial class UserAccess
{
    public int UserAccessId { get; set; }

    public int UserId { get; set; }

    public int? CompanyId { get; set; }

    public int? CompanyBranchId { get; set; }

    public bool IsDefault { get; set; }

    public bool IsActive { get; set; }

    public virtual Company? Company { get; set; }

    public virtual CompanyBranch? CompanyBranch { get; set; }

    public virtual User User { get; set; } = null!;

    public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}
