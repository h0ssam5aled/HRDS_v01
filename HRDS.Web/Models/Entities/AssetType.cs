using System;
using System.Collections.Generic;

namespace HRDS.Web.Models.Entities;

public partial class AssetType
{
    public int AssetTypeId { get; set; }

    public string AssetTypeCode { get; set; } = null!;

    public string AssetTypeNameAr { get; set; } = null!;

    public string? AssetTypeNameEn { get; set; }

    public bool IsActive { get; set; }

    public virtual ICollection<CompanyAsset> CompanyAssets { get; set; } = new List<CompanyAsset>();
}
