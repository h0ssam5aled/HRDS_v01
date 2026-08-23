namespace HRDS.Web.Areas.Finance.ViewModels
{
    public class CurrencyViewModel
    {
        public int CurrencyId { get; set; }
        public string CurrencyCode { get; set; } = null!;
        public string CurrencyNameAr { get; set; } = null!;
        public string? CurrencyNameEn { get; set; }
        public string DisplayCurrencyName { get; set; } = null!;
        public string? Symbol { get; set; }
        public string? Description { get; set; }
        public bool IsBaseCurrency { get; set; }
        public bool IsActive { get; set; }
    }
}