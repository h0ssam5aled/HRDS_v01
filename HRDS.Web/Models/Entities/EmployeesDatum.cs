using System;
using System.Collections.Generic;

namespace HRDS.Web.Models.Entities;

public partial class EmployeesDatum
{
    public int HremployeeDataId { get; set; }

    public int EmployeeId { get; set; }

    public int? CountryId { get; set; }

    public int? GovernorateId { get; set; }

    public int? CityId { get; set; }

    public string? EmployeeAddress { get; set; }

    public string? Email { get; set; }

    public string? FirstPhoneNo { get; set; }

    public string? SecondPhoneNo { get; set; }

    public string? FirstMobileNo { get; set; }

    public string? SecondMobileNo { get; set; }

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
