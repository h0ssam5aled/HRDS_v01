namespace HRDS.Web.Areas.HR.ViewModels
{
    public class DeductionTypeViewModel
    {
        public int DeductionTypeId { get; set; }
        public string DeductionTypeCode { get; set; } = null!;
        public string DeductionTypeNameAr { get; set; } = null!;
        public string? DeductionTypeNameEn { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
    }
}