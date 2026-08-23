namespace HRDS.Web.Areas.HRConfig.ViewModels
{
    public class BonusTypeViewModel
    {
        public int BonusTypeId { get; set; }
        public string BonusTypeCode { get; set; } = null!;
        public string BonusTypeNameAr { get; set; } = null!;
        public string? BonusTypeNameEn { get; set; }
        public bool IsTaxable { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
