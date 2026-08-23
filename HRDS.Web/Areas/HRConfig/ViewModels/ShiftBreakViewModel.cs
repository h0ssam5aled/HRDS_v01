namespace HRDS.Web.Areas.HRConfig.ViewModels
{
    public class ShiftBreakViewModel
    {
        public int BreakId { get; set; }
        public int ShiftId { get; set; }
        public string BreakCode { get; set; } = null!;
        public string BreakNameAr { get; set; } = null!;
        public string? BreakNameEn { get; set; }
        public string FromTime { get; set; } = null!; // HH:mm:ss
        public string ToTime { get; set; } = null!;   // HH:mm:ss
        public bool IsPaidBreak { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;
        public string? ShiftNameAr { get; set; }
    }
}
