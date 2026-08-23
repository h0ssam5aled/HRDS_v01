namespace HRDS.Web.Areas.HR.ViewModels
{
    public class HolidayCalendarViewModel
    {
        public int HolidayId { get; set; }
        public string HolidayNameAr { get; set; } = null!;
        public string? HolidayNameEn { get; set; }
        public string DisplayHolidayName { get; set; } = null!;
        public string StartDate { get; set; } = null!; // Formatted yyyy-MM-dd
        public string EndDate { get; set; } = null!;   // Formatted yyyy-MM-dd
        public short TotalDays { get; set; }
        public int Year { get; set; }
        public bool IsRecurring { get; set; }
        public bool IsActive { get; set; }
        public int? CompanyId { get; set; }
    }
}