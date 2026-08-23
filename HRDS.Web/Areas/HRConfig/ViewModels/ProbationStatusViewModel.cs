namespace HRDS.Web.Areas.HR.ViewModels
{
    public class ProbationStatusViewModel
    {
        public int ProbationStatusId { get; set; }
        public string StatusCode { get; set; } = null!;
        public string StatusNameAr { get; set; } = null!;
        public string? StatusNameEn { get; set; }
        public string? Description { get; set; }
    }
}