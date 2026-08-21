using System;
using System.Collections.Generic;

namespace HRDS.Web.Models.Entities;

public partial class EducationalInstitution
{
    public int InstitutionId { get; set; }

    public int InstitutionTypeId { get; set; }

    public string InstitutionCode { get; set; } = null!;

    public string InstitutionNameAr { get; set; } = null!;

    public string? InstitutionNameEn { get; set; }

    public virtual ICollection<AcademicFaculty> AcademicFaculties { get; set; } = new List<AcademicFaculty>();

    public virtual ICollection<EmployeeQualification> EmployeeQualifications { get; set; } = new List<EmployeeQualification>();

    public virtual EducationalInstitutionType InstitutionType { get; set; } = null!;
}
