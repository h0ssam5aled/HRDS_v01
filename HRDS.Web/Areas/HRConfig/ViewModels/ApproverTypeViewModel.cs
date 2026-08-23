namespace HRDS.Web.Areas.HR.ViewModels
{
    public class ApproverTypeViewModel
    {
        public int ApproverTypeId { get; set; }
        public string ApproverTypeCode { get; set; } = null!;
        public string ApproverTypeNameAr { get; set; } = null!;
        public string? ApproverTypeNameEn { get; set; }
        public string DisplayApproverTypeName { get; set; } = null!;
        public bool RequiresJobTitle { get; set; }
        public bool RequiresEmployee { get; set; }
        public bool IsActive { get; set; }
    }
}