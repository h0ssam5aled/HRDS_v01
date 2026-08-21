using System;
using System.Collections.Generic;

namespace HRDS.Web.Models.Entities;

public partial class EmployeeStatus
{
    public int EmployeeStatusId { get; set; }

    public string EmployeeStatusCode { get; set; } = null!;

    public string EmployeeStatusNameAr { get; set; } = null!;

    public string? EmployeeStatusNameEn { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<EmploymentHistory> EmploymentHistories { get; set; } = new List<EmploymentHistory>();
}
