using System;
using System.Collections.Generic;

namespace HRDS.Web.Models.Entities;

public partial class EmergencyContact
{
    public int EmergencyContactId { get; set; }

    public int EmployeeId { get; set; }

    public string ContactName { get; set; } = null!;

    public string? Relationship { get; set; }

    public string? PhoneNumber { get; set; }

    public string? AlternativePhone { get; set; }

    public string? MobileNumber { get; set; }

    public string? AlternativeMobileNo { get; set; }

    public bool IsPrimary { get; set; }

    public string? Notes { get; set; }

    public bool IsActive { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? DeletedBy { get; set; }

    public DateTime? DeletedAt { get; set; }

    public bool IsDeleted { get; set; }

    public virtual Employee Employee { get; set; } = null!;
}
