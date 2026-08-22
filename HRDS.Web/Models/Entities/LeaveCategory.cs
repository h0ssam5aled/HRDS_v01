using System;
using System.Collections.Generic;

namespace HRDS.Web.Models.Entities;

public partial class LeaveCategory
{
    public int LeaveCategoryId { get; set; }

    public string LeaveCategoryCode { get; set; } = null!;

    public string LeaveCategoryNameAr { get; set; } = null!;

    public string? LeaveCategoryNameEn { get; set; }

    public string? Description { get; set; }

    public bool IsActive { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? DeletedBy { get; set; }

    public DateTime? DeletedAt { get; set; }

    public bool IsDeleted { get; set; }

    public virtual ICollection<LeaveType> LeaveTypes { get; set; } = new List<LeaveType>();
}
