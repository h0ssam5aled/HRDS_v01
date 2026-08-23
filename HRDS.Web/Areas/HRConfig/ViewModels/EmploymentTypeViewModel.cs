namespace HRDS.Web.Areas.HR.ViewModels
{
    public class EmploymentTypeViewModel
    {
        public int EmploymentTypeId { get; set; }
        public string EmploymentTypeCode { get; set; } = null!;
        public string EmploymentTypeNameAr { get; set; } = null!;
        public string? EmploymentTypeNameEn { get; set; }
        public string? Description { get; set; }
        public decimal? DefaultWorkingHours { get; set; }
        public bool IsOvertimeAllowed { get; set; }
        public bool IsLeaveEligible { get; set; }
    }
}