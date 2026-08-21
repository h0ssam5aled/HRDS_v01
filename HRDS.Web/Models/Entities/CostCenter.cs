using System;
using System.Collections.Generic;

namespace HRDS.Web.Models.Entities;

public partial class CostCenter
{
    public int CostCenterId { get; set; }

    public int? ParentCostCenterId { get; set; }

    public string CostCenterCode { get; set; } = null!;

    public string CostCenterNameAr { get; set; } = null!;

    public string? CostCenterNameEn { get; set; }

    public int? CompanyId { get; set; }

    public int? CompanyBranchId { get; set; }

    public byte CostCenterLevel { get; set; }

    public bool IsLeaf { get; set; }

    public string? HierarchyPath { get; set; }

    public bool IsActive { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public virtual ICollection<CostCenter> InverseParentCostCenter { get; set; } = new List<CostCenter>();

    public virtual CostCenter? ParentCostCenter { get; set; }
}
