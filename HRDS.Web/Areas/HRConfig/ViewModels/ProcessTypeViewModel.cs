namespace HRDS.Web.Areas.HR.ViewModels
{
    public class ProcessTypeViewModel
    {
        public int ProcessTypeId { get; set; }
        public string ProcessCode { get; set; } = null!;
        public string ProcessNameAr { get; set; } = null!;
        public string? ProcessNameEn { get; set; }
        public string DisplayProcessName { get; set; } = null!;
        public bool IsActive { get; set; }
    }
}