using System;
using System.Collections.Generic;

namespace HRDS.Web.Models.Entities;

public partial class PermissionType
{
    public int PermissionTypeId { get; set; }

    public string PermissionTypeCode { get; set; } = null!;

    public string PermissionTypeNameAr { get; set; } = null!;

    public string? PermissionTypeNameEn { get; set; }

    public decimal? MaxHoursPerMonth { get; set; }

    public short? MaxCountPerMonth { get; set; }

    public bool DeductFromSalary { get; set; }

    public bool DeductFromLeaveBalance { get; set; }

    public bool RequiresAttachment { get; set; }

    public bool IsActive { get; set; }

    public virtual ICollection<PermissionRequest> PermissionRequests { get; set; } = new List<PermissionRequest>();
}
