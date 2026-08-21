using System;
using System.Collections.Generic;

namespace HRDS.Web.Models.Entities;

public partial class AcademicFaculty
{
    public int FacultyId { get; set; }

    public int InstitutionId { get; set; }

    public string FacultyCode { get; set; } = null!;

    public string FacultyNameAr { get; set; } = null!;

    public string? FacultyNameEn { get; set; }

    public virtual ICollection<AcademicMajor> AcademicMajors { get; set; } = new List<AcademicMajor>();

    public virtual ICollection<EmployeeQualification> EmployeeQualifications { get; set; } = new List<EmployeeQualification>();

    public virtual EducationalInstitution Institution { get; set; } = null!;
}
