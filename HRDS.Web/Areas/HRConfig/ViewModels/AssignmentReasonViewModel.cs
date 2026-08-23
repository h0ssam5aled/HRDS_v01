namespace HRDS.Web.Areas.HR.ViewModels
{
    public class AssignmentReasonViewModel
    {
        public int AssignmentReasonId { get; set; }
        public string AssignmentReasonCode { get; set; } = null!;
        public string AssignmentReasonNameAr { get; set; } = null!;
        public string? AssignmentReasonNameEn { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
    }
}