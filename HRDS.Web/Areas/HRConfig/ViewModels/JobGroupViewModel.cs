namespace HRDS.Web.Areas.HR.ViewModels
{
    public class JobGroupViewModel
    {
        public int JobGroupId { get; set; }
        public string JobGroupCode { get; set; } = null!;
        public string JobGroupNameAr { get; set; } = null!;
        public string? JobGroupNameEn { get; set; }
        public bool IsActive { get; set; }
    }
}