namespace HRDS.Web.Areas.HR.ViewModels
{
    public class JobLevelViewModel
    {
        public int JobLevelId { get; set; }
        public string JobLevelCode { get; set; } = null!;
        public string JobLevelNameAr { get; set; } = null!;
        public string? JobLevelNameEn { get; set; }
        public bool IsActive { get; set; }
    }
}