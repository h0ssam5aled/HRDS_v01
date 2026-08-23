namespace HRDS.Web.Areas.HR.ViewModels
{
    public class JobTitleViewModel
    {
        public int JobTitleId { get; set; }
        public int JobGroupId { get; set; }
        public string? JobGroupNameAr { get; set; }
        public string? JobGroupNameEn { get; set; }
        public string DisplayJobGroupName { get; set; } = null!;
        public string JobTitleCode { get; set; } = null!;
        public string JobTitleNameAr { get; set; } = null!;
        public string? JobTitleNameEn { get; set; }
        public string DisplayJobTitleName { get; set; } = null!;
        public bool IsActive { get; set; }
    }
}