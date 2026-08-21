using System;
using System.Collections.Generic;

namespace HRDS.Web.Models.Entities;

public partial class AcademicMajor
{
    public int MajorId { get; set; }

    public int FacultyId { get; set; }

    public string MajorCode { get; set; } = null!;

    public string MajorNameAr { get; set; } = null!;

    public string? MajorNameEn { get; set; }

    public virtual ICollection<EmployeeQualification> EmployeeQualifications { get; set; } = new List<EmployeeQualification>();

    public virtual AcademicFaculty Faculty { get; set; } = null!;
}
