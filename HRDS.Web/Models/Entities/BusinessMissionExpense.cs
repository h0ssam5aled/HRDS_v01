using System;
using System.Collections.Generic;

namespace HRDS.Web.Models.Entities;

public partial class BusinessMissionExpense
{
    public int ExpenseId { get; set; }

    public int MissionRequestId { get; set; }

    public int ExpenseTypeId { get; set; }

    public decimal Amount { get; set; }

    public int? CurrencyId { get; set; }

    public DateOnly ExpenseDate { get; set; }

    public string? ReceiptNumber { get; set; }

    public string? AttachmentPath { get; set; }

    public string? Notes { get; set; }

    public bool IsApproved { get; set; }

    public decimal? ApprovedAmount { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? DeletedBy { get; set; }

    public DateTime? DeletedAt { get; set; }

    public bool IsDeleted { get; set; }

    public virtual ExpenseType ExpenseType { get; set; } = null!;

    public virtual BusinessMissionRequest MissionRequest { get; set; } = null!;
}
