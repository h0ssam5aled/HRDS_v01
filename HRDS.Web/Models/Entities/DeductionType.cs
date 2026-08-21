using System;
using System.Collections.Generic;

namespace HRDS.Web.Models.Entities;

public partial class DeductionType
{
    public int DeductionTypeId { get; set; }

    public string DeductionTypeCode { get; set; } = null!;

    public string DeductionTypeNameAr { get; set; } = null!;

    public string? DeductionTypeNameEn { get; set; }

    public string? Description { get; set; }

    public bool IsActive { get; set; }

    public virtual ICollection<EmployeeDeduction> EmployeeDeductions { get; set; } = new List<EmployeeDeduction>();
}
