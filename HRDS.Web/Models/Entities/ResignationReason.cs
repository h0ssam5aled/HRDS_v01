using System;
using System.Collections.Generic;

namespace HRDS.Web.Models.Entities;

public partial class ResignationReason
{
    public int ResignationReasonId { get; set; }

    public string ResignationReasonCode { get; set; } = null!;

    public string ResignationReasonNameAr { get; set; } = null!;

    public string? ResignationReasonNameEn { get; set; }

    public string? Description { get; set; }

    public bool IsActive { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? DeletedBy { get; set; }

    public DateTime? DeletedAt { get; set; }

    public bool IsDeleted { get; set; }
}
