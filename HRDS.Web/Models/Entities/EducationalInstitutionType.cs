using System;
using System.Collections.Generic;

namespace HRDS.Web.Models.Entities;

public partial class EducationalInstitutionType
{
    public int InstitutionTypeId { get; set; }

    public string InstitutionTypeCode { get; set; } = null!;

    public string InstitutionTypeNameAr { get; set; } = null!;

    public string? InstitutionTypeNameEn { get; set; }

    public virtual ICollection<EducationalInstitution> EducationalInstitutions { get; set; } = new List<EducationalInstitution>();
}
