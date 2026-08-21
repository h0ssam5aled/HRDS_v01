using System;
using System.Collections.Generic;

namespace HRDS.Web.Models.Entities;

public partial class AllowanceType
{
    public int AllowanceTypeId { get; set; }

    public string AllowanceTypeCode { get; set; } = null!;

    public string AllowanceTypeNameAr { get; set; } = null!;

    public string? AllowanceTypeNameEn { get; set; }

    public string? Description { get; set; }

    public bool IsActive { get; set; }

    public virtual ICollection<EmployeeAllowance> EmployeeAllowances { get; set; } = new List<EmployeeAllowance>();
}
