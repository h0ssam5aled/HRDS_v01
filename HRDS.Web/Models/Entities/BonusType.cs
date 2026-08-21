using System;
using System.Collections.Generic;

namespace HRDS.Web.Models.Entities;

public partial class BonusType
{
    public int BonusTypeId { get; set; }

    public string BonusTypeCode { get; set; } = null!;

    public string BonusTypeNameAr { get; set; } = null!;

    public string? BonusTypeNameEn { get; set; }

    public bool IsTaxable { get; set; }

    public bool IsActive { get; set; }

    public virtual ICollection<EmployeeBonuse> EmployeeBonuses { get; set; } = new List<EmployeeBonuse>();
}
