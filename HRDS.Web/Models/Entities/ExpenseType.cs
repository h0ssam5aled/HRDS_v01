using System;
using System.Collections.Generic;

namespace HRDS.Web.Models.Entities;

public partial class ExpenseType
{
    public int ExpenseTypeId { get; set; }

    public string ExpenseTypeCode { get; set; } = null!;

    public string ExpenseTypeNameAr { get; set; } = null!;

    public string? ExpenseTypeNameEn { get; set; }

    public string? Description { get; set; }

    public bool RequiresAttachment { get; set; }

    public decimal? MaxLimit { get; set; }

    public bool IsActive { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? DeletedBy { get; set; }

    public DateTime? DeletedAt { get; set; }

    public bool IsDeleted { get; set; }

    public virtual ICollection<BusinessMissionExpense> BusinessMissionExpenses { get; set; } = new List<BusinessMissionExpense>();
}
