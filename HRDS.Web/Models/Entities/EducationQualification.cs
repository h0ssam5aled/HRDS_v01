using System;
using System.Collections.Generic;

namespace HRDS.Web.Models.Entities;

public partial class EducationQualification
{
    public int QualificationId { get; set; }

    public string QualificationCode { get; set; } = null!;

    public string QualificationNameAr { get; set; } = null!;

    public string? QualificationNameEn { get; set; }

    public virtual ICollection<EmployeeQualification> EmployeeQualifications { get; set; } = new List<EmployeeQualification>();
}
