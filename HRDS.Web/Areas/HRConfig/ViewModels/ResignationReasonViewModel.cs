namespace HRDS.Web.Areas.HRConfig.ViewModels
{
    public class ResignationReasonViewModel
    {
        public int ResignationReasonId { get; set; }
        public string ResignationReasonCode { get; set; } = null!;
        public string ResignationReasonNameAr { get; set; } = null!;
        public string? ResignationReasonNameEn { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;
    }
}