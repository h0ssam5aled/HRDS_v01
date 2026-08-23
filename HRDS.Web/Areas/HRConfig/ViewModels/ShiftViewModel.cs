namespace HRDS.Web.Areas.HR.ViewModels
{
    public class ShiftViewModel
    {
        public int ShiftId { get; set; }
        public int ShiftTypeId { get; set; }
        public string? ShiftTypeNameAr { get; set; }
        public string ShiftCode { get; set; } = null!;
        public string ShiftNameAr { get; set; } = null!;
        public string? ShiftNameEn { get; set; }

        // التعديل هنا: TimeOnly بدلاً من TimeSpan
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }

        public bool CrossMidnight { get; set; }
        public int GraceInMinutes { get; set; }
        public int GraceOutMinutes { get; set; }
        public bool AllowOvertime { get; set; }
        public int MinimumOvertimeMinutes { get; set; }
        public bool AllowLateDeduction { get; set; }
        public bool AutoCloseAttendance { get; set; }
        public decimal? StandardHours { get; set; }
        public bool IsActive { get; set; }
    }
}