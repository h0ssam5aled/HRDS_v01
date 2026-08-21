using System;
using System.Collections.Generic;

namespace HRDS.Web.Models.Entities;

public partial class ShiftType
{
    public int ShiftTypeId { get; set; }

    public string ShiftTypeCode { get; set; } = null!;

    public string ShiftTypeNameAr { get; set; } = null!;

    public string? ShiftTypeNameEn { get; set; }

    public string? Description { get; set; }

    public bool IsActive { get; set; }

    public virtual ICollection<Shift> Shifts { get; set; } = new List<Shift>();
}
