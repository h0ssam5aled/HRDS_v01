namespace HRDS.Web.Areas.HR.ViewModels
{
    public class PositionViewModel
    {
        public int PositionId { get; set; }
        public int? UnitId { get; set; }
        public string? UnitName { get; set; }
        public int? ReportsToPositionId { get; set; }
        public string? ReportsToPositionName { get; set; }
        public int JobTitleId { get; set; }
        public string DisplayJobTitle { get; set; } = null!;
        public int? JobLevelId { get; set; }
        public string? DisplayJobLevel { get; set; }
        public int PositionStatusId { get; set; }
        public string DisplayPositionStatus { get; set; } = null!;
        public string PositionCode { get; set; } = null!;
        public string PositionNameAr { get; set; } = null!;
        public string? PositionNameEn { get; set; }
        public string DisplayPositionName { get; set; } = null!;
        public short? HeadCount { get; set; }
        public bool IsManagerial { get; set; }
        public string? EffectiveFrom { get; set; }
        public string? EffectiveTo { get; set; }
        public string? Remarks { get; set; }
        public bool IsActive { get; set; }
    }
}