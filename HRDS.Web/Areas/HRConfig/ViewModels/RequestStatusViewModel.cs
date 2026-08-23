namespace HRDS.Web.Areas.HR.ViewModels
{
    public class RequestStatusViewModel
    {
        public int StatusId { get; set; }
        public string StatusCode { get; set; } = null!;
        public string StatusNameAr { get; set; } = null!;
        public string? StatusNameEn { get; set; }
        public string DisplayStatusName { get; set; } = null!;
        public string? BadgeClass { get; set; }
        public bool IsFinal { get; set; }
        public bool IsActive { get; set; }
    }
}