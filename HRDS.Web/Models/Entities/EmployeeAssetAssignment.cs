using System;
using System.Collections.Generic;

namespace HRDS.Web.Models.Entities;

public partial class EmployeeAssetAssignment
{
    public int AssignmentId { get; set; }

    public int AssetId { get; set; }

    public int EmployeeId { get; set; }

    public DateOnly AssignedDate { get; set; }

    public DateOnly? ExpectedReturnDate { get; set; }

    public DateOnly? ActualReturnDate { get; set; }

    public string? ConditionOnAssignment { get; set; }

    public string? ConditionOnReturn { get; set; }

    public bool IsReturned { get; set; }

    public string? Notes { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual CompanyAsset Asset { get; set; } = null!;

    public virtual Employee Employee { get; set; } = null!;
}
