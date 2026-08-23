namespace HRDS.Web.Areas.HRConfig.ViewModels
{
    public class PenaltyTypeViewModel
    {
        public int PenaltyTypeId { get; set; }
        public string PenaltyTypeCode { get; set; } = null!;
        public string PenaltyTypeNameAr { get; set; } = null!;
        public string? PenaltyTypeNameEn { get; set; }
        public decimal? DefaultDeductionDays { get; set; }
        public bool IsActive { get; set; } = true;
    }
}