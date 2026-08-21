using System;
using System.Collections.Generic;

namespace HRDS.Web.Models.Entities;

public partial class BusinessMissionType
{
    public int MissionTypeId { get; set; }

    public string MissionTypeCode { get; set; } = null!;

    public string MissionTypeNameAr { get; set; } = null!;

    public string? MissionTypeNameEn { get; set; }

    public bool HasAllowance { get; set; }

    public bool IsActive { get; set; }

    public virtual ICollection<BusinessMissionRequest> BusinessMissionRequests { get; set; } = new List<BusinessMissionRequest>();
}
