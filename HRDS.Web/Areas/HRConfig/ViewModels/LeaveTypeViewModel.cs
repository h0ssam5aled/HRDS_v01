namespace HRDS.Web.Areas.HRConfig.ViewModels
{
    public class LeaveTypeViewModel
    {
        public int LeaveTypeId { get; set; }
        public string LeaveCode { get; set; } = null!;
        public string LeaveNameAr { get; set; } = null!;
        public string? LeaveNameEn { get; set; }
        public int? LeaveCategoryId { get; set; }
        public string? LeaveCategoryName { get; set; }

        // Rules & Balances
        public bool IsPaid { get; set; }
        public bool RequiresBalance { get; set; }
        public bool RequiresAttachment { get; set; }
        public bool RequiresApproval { get; set; }
        public bool RequiresWorkflow { get; set; }
        public bool DeductFromSalary { get; set; }

        // Limits
        public decimal? MaxDaysPerRequest { get; set; }
        public decimal? MaxDaysPerYear { get; set; }
        public decimal? MinimumDaysPerRequest { get; set; }
        public decimal? MaximumConsecutiveDays { get; set; }

        // Advance & Carry Forward
        public bool AllowCarryForward { get; set; }
        public decimal? CarryForwardLimit { get; set; }
        public bool ExpireAtYearEnd { get; set; }
        public bool AllowBackDateRequest { get; set; }
        public bool AllowFutureRequest { get; set; }
        public short? MaxFutureDays { get; set; }

        // UI & General
        public string? ColorCode { get; set; }
        public string? IconName { get; set; }
        public short? DisplayOrder { get; set; }
        public bool IsActive { get; set; } = true;
    }
}