using System;
using System.Collections.Generic;

namespace HRDS.Web.Models.Entities;

public partial class Unit
{
    public int UnitId { get; set; }

    public int SectionId { get; set; }

    public string UnitCode { get; set; } = null!;

    public string UnitNameAr { get; set; } = null!;

    public string? UnitNameEn { get; set; }

    public int? DisplayOrder { get; set; }

    public string? Description { get; set; }

    public bool IsActive { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? DeletedBy { get; set; }

    public DateTime? DeletedAt { get; set; }

    public bool IsDeleted { get; set; }

    public virtual ICollection<Position> Positions { get; set; } = new List<Position>();

    public virtual Section Section { get; set; } = null!;
}
